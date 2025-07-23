// 2. MaintenanceService Implementation
using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Dashboard;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.Interfaces.Services;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;

namespace ERP.Application.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public MaintenanceService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result<MaintenanceDto>> CreateMaintenanceAsync(CreateMaintenanceDto dto)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                {
                    return Result<MaintenanceDto>.Failure("Araç bulunamadı.");
                }

                var maintenance = new VehicleMaintenance(
                    dto.VehicleId,
                    dto.Type,
                    dto.ScheduledDate,
                    dto.VehicleKm,
                    dto.Description,
                    dto.ServiceProvider
                );

                if (dto.EstimatedCost.HasValue)
                {
                    maintenance.SetCost(dto.EstimatedCost.Value);
                }

                await _unitOfWork.VehicleMaintenances.AddAsync(maintenance);
                await _unitOfWork.SaveChangesAsync();

                var maintenanceDto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(maintenanceDto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceDto>> UpdateMaintenanceAsync(int id, UpdateMaintenanceDto dto)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(id);
                if (maintenance == null)
                {
                    return Result<MaintenanceDto>.Failure("Bakım bulunamadı.");
                }

                if (maintenance.IsCompleted)
                {
                    return Result<MaintenanceDto>.Failure("Tamamlanmış bakım güncellenemez.");
                }

                maintenance.SetDescription(dto.Description);
                if (dto.Cost.HasValue)
                {
                    maintenance.SetCost(dto.Cost.Value);
                }

                await _unitOfWork.VehicleMaintenances.UpdateAsync(maintenance);
                await _unitOfWork.SaveChangesAsync();

                var maintenanceDto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(maintenanceDto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım güncellenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceDto>> GetMaintenanceByIdAsync(int id)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(id);
                if (maintenance == null)
                {
                    return Result<MaintenanceDto>.Failure("Bakım bulunamadı.");
                }

                var maintenanceDto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(maintenanceDto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım getirilirken hata: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<MaintenanceDto>> GetMaintenancesByVehicleAsync(int vehicleId, PaginationDto pagination)
        {
            try
            {
                var maintenances = await _unitOfWork.VehicleMaintenances.GetByVehicleAsync(vehicleId);

                var totalCount = maintenances.Count();
                var pagedMaintenances = maintenances
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize);

                var maintenanceDtos = _mapper.Map<IEnumerable<MaintenanceDto>>(pagedMaintenances);

                return new PaginatedResult<MaintenanceDto>
                {
                    Items = maintenanceDtos,
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<MaintenanceDto>
                {
                    Items = new List<MaintenanceDto>(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };
            }
        }

        public async Task<Result<MaintenanceDto>> CompleteMaintenanceAsync(int id, CompleteMaintenanceDto dto)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(id);
                if (maintenance == null)
                {
                    return Result<MaintenanceDto>.Failure("Bakım bulunamadı.");
                }

                maintenance.Complete(dto.CompletedDate, dto.ActualCost, dto.CompletionNotes);

                if (dto.NextMaintenanceDate.HasValue || dto.NextMaintenanceKm.HasValue)
                {
                    maintenance.ScheduleNextMaintenance(dto.NextMaintenanceDate, dto.NextMaintenanceKm);
                }

                await _unitOfWork.VehicleMaintenances.UpdateAsync(maintenance);
                await _unitOfWork.SaveChangesAsync();

                // Create notification for completion
                if (dto.CreateReminder && dto.NextMaintenanceDate.HasValue)
                {
                    await _notificationService.ScheduleMaintenanceReminderAsync(id, dto.NextMaintenanceDate.Value.AddDays(-7));
                }

                var maintenanceDto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(maintenanceDto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım tamamlanırken hata: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceDto>> ScheduleMaintenanceAsync(ScheduleMaintenanceDto dto)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                {
                    return Result<MaintenanceDto>.Failure("Araç bulunamadı.");
                }

                var maintenanceType = Enum.Parse<MaintenanceType>(dto.MaintenanceType);
                var maintenance = new VehicleMaintenance(
                    dto.VehicleId,
                    maintenanceType,
                    dto.ScheduledDate,
                    dto.VehicleKm,
                    dto.Description,
                    dto.ServiceProvider
                );

                if (dto.EstimatedCost.HasValue)
                {
                    maintenance.SetCost(dto.EstimatedCost.Value);
                }

                await _unitOfWork.VehicleMaintenances.AddAsync(maintenance);
                await _unitOfWork.SaveChangesAsync();

                // Create reminder notification
                if (dto.CreateReminder)
                {
                    var reminderDate = dto.ScheduledDate.AddDays(-dto.ReminderDaysBefore);
                    await _notificationService.ScheduleMaintenanceReminderAsync(maintenance.Id, reminderDate);
                }

                var maintenanceDto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(maintenanceDto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım planlanırken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteMaintenanceAsync(int id)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(id);
                if (maintenance == null)
                {
                    return Result<bool>.Failure("Bakım bulunamadı.");
                }

                if (maintenance.IsCompleted)
                {
                    return Result<bool>.Failure("Tamamlanmış bakım silinemez.");
                }

                await _unitOfWork.VehicleMaintenances.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Bakım silinirken hata: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UpcomingMaintenanceDto>> GetUpcomingMaintenanceAsync(int daysAhead = 30)
        {
            try
            {
                var maintenances = await _unitOfWork.VehicleMaintenances.GetUpcomingMaintenanceAsync();
                var upcomingMaintenances = maintenances.Where(m =>
                    !m.IsCompleted &&
                    m.ScheduledDate <= DateTime.Today.AddDays(daysAhead));

                return _mapper.Map<IEnumerable<UpcomingMaintenanceDto>>(upcomingMaintenances);
            }
            catch (Exception ex)
            {
                return new List<UpcomingMaintenanceDto>();
            }
        }

        public async Task<IEnumerable<MaintenanceDto>> GetOverdueMaintenanceAsync()
        {
            try
            {
                var maintenances = await _unitOfWork.VehicleMaintenances.GetOverdueMaintenanceAsync();
                return _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
            }
            catch (Exception ex)
            {
                return new List<MaintenanceDto>();
            }
        }

        // Placeholder implementations for complex reporting methods
        public async Task<MaintenancePerformanceReportDto> GetMaintenancePerformanceReportAsync(DateTime startDate, DateTime endDate)
        {
            // Complex reporting logic would go here
            return new MaintenancePerformanceReportDto
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public async Task<MaintenanceDashboardStats> GetMaintenanceDashboardStatsAsync()
        {
            try
            {
                var maintenances = await _unitOfWork.VehicleMaintenances.GetAllAsync();

                return new MaintenanceDashboardStats
                {
                    TotalMaintenances = maintenances.Count(),
                    CompletedMaintenances = maintenances.Count(m => m.IsCompleted),
                    PendingMaintenances = maintenances.Count(m => !m.IsCompleted && m.ScheduledDate >= DateTime.Today),
                    //OverdueMaintenances = maintenances.Count(m => !m.IsCompleted && m.ScheduledDate < DateTime.Today),
                    TotalMaintenanceCost = maintenances.Where(m => m.Cost.HasValue).Sum(m => m.Cost.Value),
                    AverageMaintenanceCost = maintenances.Where(m => m.Cost.HasValue).Any() ?
                        maintenances.Where(m => m.Cost.HasValue).Average(m => m.Cost.Value) : 0
                };
            }
            catch (Exception ex)
            {
                return new MaintenanceDashboardStats();
            }
        }

        // Placeholder implementations for remaining methods
        public async Task<IEnumerable<MaintenanceKPIDto>> GetMaintenanceKPIsAsync(DateTime startDate, DateTime endDate)
        {
            return new List<MaintenanceKPIDto>();
        }

        public async Task<IEnumerable<ServiceProviderPerformanceDto>> GetServiceProviderPerformanceAsync(DateTime startDate, DateTime endDate)
        {
            return new List<ServiceProviderPerformanceDto>();
        }

        public async Task<MaintenanceEfficiencyDto> GetMaintenanceEfficiencyAsync(DateTime startDate, DateTime endDate)
        {
            return new MaintenanceEfficiencyDto
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public async Task<IEnumerable<EfficiencyImprovementDto>> GetEfficiencyImprovementSuggestionsAsync()
        {
            return new List<EfficiencyImprovementDto>();
        }
    }
}