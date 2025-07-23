using ERP.Application.DTOs.Vehicle;
using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Core.Interfaces;
using ERP.Core.Entities;
using ERP.Core.Enums;
using AutoMapper;
using ERP.Application.Interfaces.Infrastructure;

namespace ERP.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;

        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<Result<VehicleDto>> GetVehicleByIdAsync(int id)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<VehicleDto>.Failure("Araç bulunamadı.");
                }

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<VehicleDto>> GetVehicleByPlateNumberAsync(string plateNumber)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByPlateNumberAsync(plateNumber);
                if (vehicle == null)
                {
                    return Result<VehicleDto>.Failure("Araç bulunamadı.");
                }

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<VehicleDto>> GetVehicleByVinNumberAsync(string vinNumber)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByVinNumberAsync(vinNumber);
                if (vehicle == null)
                {
                    return Result<VehicleDto>.Failure("Araç bulunamadı.");
                }

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç getirilirken hata: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<VehicleListDto>> GetVehiclesPagedAsync(VehicleFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    vehicles = vehicles.Where(v =>
                        v.PlateNumber.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        v.Brand.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        v.Model.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (filter.Status.HasValue)
                {
                    vehicles = vehicles.Where(v => v.Status == filter.Status.Value);
                }

                if (filter.Type.HasValue)
                {
                    vehicles = vehicles.Where(v => v.Type == filter.Type.Value);
                }

                if (filter.UserId.HasValue)
                {
                    vehicles = vehicles.Where(v => v.AssignedUserId == filter.UserId.Value);
                }

                if (filter.Year.HasValue)
                {
                    vehicles = vehicles.Where(v => v.Year == filter.Year.Value);
                }

                if (!string.IsNullOrEmpty(filter.Brand))
                {
                    vehicles = vehicles.Where(v => v.Brand.Contains(filter.Brand, StringComparison.OrdinalIgnoreCase));
                }

                var totalCount = vehicles.Count();
                var pagedVehicles = vehicles
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var vehicleDtos = _mapper.Map<IEnumerable<VehicleListDto>>(pagedVehicles);

                return new PaginatedResult<VehicleListDto>
                {
                    Items = vehicleDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<VehicleListDto>
                {
                    Items = new List<VehicleListDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetAllVehiclesAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAllAsync();
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Araçlar getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetAvailableVehiclesAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAvailableVehiclesAsync();
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Müsait araçlar getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetAssignedVehiclesAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAssignedVehiclesAsync();
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Atanmış araçlar getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehiclesByUserAsync(int userId)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetVehiclesByUserAsync(userId);
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Kullanıcı araçları getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehiclesByStatusAsync(VehicleStatus status)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetByStatusAsync(status);
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Durum araçları getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehiclesNeedingMaintenanceAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetVehiclesNeedingMaintenanceAsync();
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Bakım gereken araçlar getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehiclesNeedingInspectionAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetVehiclesNeedingInspectionAsync();
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Muayene gereken araçlar getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<VehicleDto>> CreateVehicleAsync(CreateVehicleDto dto)
        {
            try
            {
                // Plate number uniqueness check
                var plateExists = await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(dto.PlateNumber);
                if (plateExists)
                {
                    return Result<VehicleDto>.Failure("Bu plaka numarası zaten kullanılıyor.");
                }

                // VIN number uniqueness check
                var vinExists = await _unitOfWork.Vehicles.IsVinNumberExistsAsync(dto.VinNumber);
                if (vinExists)
                {
                    return Result<VehicleDto>.Failure("Bu VIN numarası zaten kullanılıyor.");
                }

                var vehicleType = Enum.Parse<VehicleType>(dto.Type);
                var fuelType = Enum.Parse<FuelType>(dto.FuelType);

                var vehicle = new Vehicle(
                    dto.PlateNumber,
                    dto.VinNumber,
                    dto.Brand,
                    dto.Model,
                    dto.Year,
                    vehicleType,
                    fuelType,
                    dto.PurchaseDate,
                    dto.PurchasePrice
                );

                // Set additional properties
                vehicle.UpdateKilometer(dto.CurrentKm);

                if (!string.IsNullOrEmpty(dto.Notes))
                {
                    vehicle.SetNotes(dto.Notes);
                }

                await _unitOfWork.Vehicles.AddAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<VehicleDto>> UpdateVehicleAsync(int id, UpdateVehicleDto dto)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<VehicleDto>.Failure("Araç bulunamadı.");
                }

                // Plate number uniqueness check
                if (vehicle.PlateNumber != dto.PlateNumber)
                {
                    var plateExists = await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(dto.PlateNumber, id);
                    if (plateExists)
                    {
                        return Result<VehicleDto>.Failure("Bu plaka numarası zaten kullanılıyor.");
                    }
                }

                // VIN number uniqueness check
                if (vehicle.VinNumber != dto.VinNumber)
                {
                    var vinExists = await _unitOfWork.Vehicles.IsVinNumberExistsAsync(dto.VinNumber, id);
                    if (vinExists)
                    {
                        return Result<VehicleDto>.Failure("Bu VIN numarası zaten kullanılıyor.");
                    }
                }

                vehicle.SetPlateNumber(dto.PlateNumber);
                vehicle.SetVinNumber(dto.VinNumber);
                vehicle.SetBrandModel(dto.Brand, dto.Model);
                vehicle.SetYear(dto.Year);

                if (dto.CurrentKm > vehicle.CurrentKm)
                {
                    vehicle.UpdateKilometer(dto.CurrentKm);
                }

                if (!string.IsNullOrEmpty(dto.Notes))
                {
                    vehicle.SetNotes(dto.Notes);
                }

                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(vehicleDto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç güncellenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteVehicleAsync(int id)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                if (vehicle.AssignedUserId.HasValue)
                {
                    return Result<bool>.Failure("Atanmış araç silinemez. Önce aracı geri alın.");
                }

                vehicle.MarkAsDeleted();
                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç silinirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> AssignVehicleToUserAsync(int vehicleId, int userId)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result<bool>.Failure("Kullanıcı bulunamadı.");
                }

                vehicle.AssignToUser(userId);
                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                // Create notification
                await _notificationService.CreateNotificationAsync(new ERP.Application.DTOs.Notification.CreateNotificationDto
                {
                    VehicleId = vehicleId,
                    UserId = userId,
                    Type =  NotificationType.VehicleAssigned ,
                    Title = "Araç Atandı",
                    Message = $"{vehicle.PlateNumber} plakalı araç size atanmıştır.",
                    ScheduledDate = DateTime.UtcNow,
                    Priority = 2
                });

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç atanırken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ReturnVehicleFromUserAsync(int vehicleId, string? notes = null)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                if (!vehicle.AssignedUserId.HasValue)
                {
                    return Result<bool>.Failure("Araç zaten atanmamış.");
                }

                var userId = vehicle.AssignedUserId.Value;
                vehicle.ReturnFromUser();

                if (!string.IsNullOrEmpty(notes))
                {
                    vehicle.SetNotes(notes);
                }

                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                // Create notification
                await _notificationService.CreateNotificationAsync(new ERP.Application.DTOs.Notification.CreateNotificationDto
                {
                    VehicleId = vehicleId,
                    UserId = userId,
                    Type =  NotificationType.OverdueReturn, 
                    Title = "Araç Teslim Edildi",
                    Message = $"{vehicle.PlateNumber} plakalı araç teslim edilmiştir.",
                    ScheduledDate = DateTime.UtcNow,
                    Priority = 1
                });

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç geri alınırken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateVehicleStatusAsync(int id, VehicleStatus status, string? reason = null)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                vehicle.SetStatus(status);

                if (!string.IsNullOrEmpty(reason))
                {
                    vehicle.SetNotes($"Durum değişikliği: {reason}");
                }

                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç durumu güncellenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateVehicleKilometerAsync(int id, decimal km, string? notes = null)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                vehicle.UpdateKilometer(km);

                if (!string.IsNullOrEmpty(notes))
                {
                    vehicle.SetNotes(notes);
                }

                await _unitOfWork.Vehicles.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç kilometresi güncellenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsPlateNumberUniqueAsync(string plateNumber, int? excludeVehicleId = null)
        {
            try
            {
                var exists = await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(plateNumber, excludeVehicleId);
                return Result<bool>.Success(!exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Plaka benzersizliği kontrol edilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsVinNumberUniqueAsync(string vinNumber, int? excludeVehicleId = null)
        {
            try
            {
                var exists = await _unitOfWork.Vehicles.IsVinNumberExistsAsync(vinNumber, excludeVehicleId);
                return Result<bool>.Success(!exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"VIN benzersizliği kontrol edilirken hata: {ex.Message}");
            }
        }

        // Placeholder implementations for remaining methods
        public async Task<Result<IEnumerable<VehicleDto>>> GetVehicleAssignmentHistoryAsync(int vehicleId)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehicleLocationHistoryAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> SendVehicleToMaintenanceAsync(int vehicleId, string reason, DateTime? scheduledDate = null)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ReturnVehicleFromMaintenanceAsync(int vehicleId, string? notes = null)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<VehicleStatisticsDto>> GetVehicleStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return Result<VehicleStatisticsDto>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetFleetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetFleetPerformanceReportAsync(VehicleReportFilterDto filter)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehicleUtilizationReportAsync(VehicleReportFilterDto filter)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> ExportVehiclesAsync(VehicleExportFilterDto filter)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ImportVehiclesAsync(byte[] data, VehicleImportFormat format)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> CreateMultipleVehiclesAsync(IEnumerable<CreateVehicleDto> vehicles)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> UpdateMultipleVehiclesAsync(IEnumerable<UpdateVehicleBulkDto> vehicles)
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<int>>> DeleteMultipleVehiclesAsync(IEnumerable<int> vehicleIds)
        {
            return Result<IEnumerable<int>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> vehicleIds, VehicleStatus status)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetVehicleAlertsAsync()
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<VehicleDto>>> GetCriticalVehicleAlertsAsync()
        {
            return Result<IEnumerable<VehicleDto>>.Failure("Method not implemented yet");
        }
    }
}