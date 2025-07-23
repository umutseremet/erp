using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class VehicleLocationHistoryRepository : BaseRepository<VehicleLocationHistory>, IVehicleLocationHistoryRepository
    {
        public VehicleLocationHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleLocationHistory>> GetByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(vlh => vlh.Vehicle)
                .Where(vlh => vlh.VehicleId == vehicleId)
                .OrderByDescending(vlh => vlh.RecordedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleLocationHistory>> GetByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(vlh => vlh.Vehicle)
                .Where(vlh => vlh.VehicleId == vehicleId &&
                             vlh.RecordedAt >= startDate &&
                             vlh.RecordedAt <= endDate)
                .OrderBy(vlh => vlh.RecordedAt)
                .ToListAsync();
        }

        public async Task<VehicleLocationHistory?> GetLastLocationAsync(int vehicleId)
        {
            return await _dbSet
                .Include(vlh => vlh.Vehicle)
                .Where(vlh => vlh.VehicleId == vehicleId)
                .OrderByDescending(vlh => vlh.RecordedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VehicleLocationHistory>> GetLocationsByAreaAsync(decimal centerLat, decimal centerLon, decimal radiusKm)
        {
            // Basit bir yaklaþým - daha geliþmiþ spatial queries için PostGIS kullanýlabilir
            var latDelta = radiusKm / 111m; // 1 derece ? 111 km
            var lonDelta = radiusKm / (111m * (decimal)Math.Cos((double)centerLat * Math.PI / 180));

            return await _dbSet
                .Include(vlh => vlh.Vehicle)
                .Where(vlh => vlh.Latitude >= centerLat - latDelta &&
                             vlh.Latitude <= centerLat + latDelta &&
                             vlh.Longitude >= centerLon - lonDelta &&
                             vlh.Longitude <= centerLon + lonDelta)
                .OrderByDescending(vlh => vlh.RecordedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalDistanceAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var locations = await GetByVehicleAndDateRangeAsync(vehicleId, startDate, endDate);
            var locationList = locations.OrderBy(l => l.RecordedAt).ToList();

            decimal totalDistance = 0;

            for (int i = 1; i < locationList.Count; i++)
            {
                var distance = VehicleLocationHistory.CalculateDistance(locationList[i - 1], locationList[i]);
                totalDistance += (decimal)distance;
            }

            return totalDistance;
        }

        public async Task DeleteOldLocationsAsync(int vehicleId, DateTime olderThan)
        {
            var oldLocations = await _dbSet
                .Where(vlh => vlh.VehicleId == vehicleId && vlh.RecordedAt < olderThan)
                .ToListAsync();

            foreach (var location in oldLocations)
            {
                location.MarkAsDeleted();
            }
        }
    }
}