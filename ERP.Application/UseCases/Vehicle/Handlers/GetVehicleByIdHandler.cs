using AutoMapper;
using ERP.Application.Common.Constants;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Application.UseCases.Vehicle.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class GetVehicleByIdHandler : IRequestHandler<GetVehicleByIdQuery, Result<VehicleDetailDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetVehicleByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<VehicleDetailDto?>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = CacheKeys.VehicleById(request.Id);
                var cachedVehicle = await _cacheService.GetAsync<VehicleDetailDto>(cacheKey);

                if (cachedVehicle != null)
                    return Result<VehicleDetailDto?>.Success(cachedVehicle);

                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
                if (vehicle == null)
                    return Result<VehicleDetailDto?>.Success(null);

                var vehicleDetail = _mapper.Map<VehicleDetailDto>(vehicle);

                // Load related data
                vehicleDetail.MaintenanceHistory = _mapper.Map<List<VehicleMaintenanceDto>>(vehicle.Maintenances);
                vehicleDetail.Inspections = _mapper.Map<List<VehicleInspectionDto>>(vehicle.Inspections);
                vehicleDetail.Licenses = _mapper.Map<List<VehicleLicenseDto>>(vehicle.Licenses);
                vehicleDetail.RecentFuelTransactions = _mapper.Map<List<FuelTransactionSummaryDto>>(
                    vehicle.FuelTransactions.OrderByDescending(f => f.TransactionDate).Take(10));

                await _cacheService.SetAsync(cacheKey, vehicleDetail, TimeSpan.FromMinutes(30));

                return Result<VehicleDetailDto?>.Success(vehicleDetail);
            }
            catch (Exception ex)
            {
                return Result<VehicleDetailDto?>.Failure($"Araç detayları getirilirken hata: {ex.Message}");
            }
        }
    }
}