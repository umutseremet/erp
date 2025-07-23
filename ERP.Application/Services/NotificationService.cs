using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Notification;
using ERP.Application.Extensions;
using ERP.Application.Interfaces.External;
using ERP.Application.Interfaces.Services;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;

namespace ERP.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task<Result<NotificationDto>> CreateNotificationAsync(CreateNotificationDto dto)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                {
                    return Result<NotificationDto>.Failure("Araç bulunamadı.");
                }
                 
                var notification = new VehicleNotification(
                    dto.VehicleId,
                    dto.Type,
                    dto.Title,
                    dto.Message,
                    dto.ScheduledDate,
                    dto.UserId,
                    dto.Priority
                );

                if (!string.IsNullOrEmpty(dto.ActionUrl))
                {
                    notification.SetActionUrl(dto.ActionUrl);
                }

                if (!string.IsNullOrEmpty(dto.AdditionalData))
                {
                    notification.SetAdditionalData(dto.AdditionalData);
                }

                await _unitOfWork.VehicleNotifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                var notificationDto = _mapper.Map<NotificationDto>(notification);
                return Result<NotificationDto>.Success(notificationDto);
            }
            catch (Exception ex)
            {
                return Result<NotificationDto>.Failure($"Bildirim oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<NotificationDto>> GetNotificationByIdAsync(int id)
        {
            try
            {
                var notification = await _unitOfWork.VehicleNotifications.GetByIdAsync(id);
                if (notification == null)
                {
                    return Result<NotificationDto>.Failure("Bildirim bulunamadı.");
                }

                var notificationDto = _mapper.Map<NotificationDto>(notification);
                return Result<NotificationDto>.Success(notificationDto);
            }
            catch (Exception ex)
            {
                return Result<NotificationDto>.Failure($"Bildirim getirilirken hata: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<NotificationDto>> GetNotificationsByUserAsync(int userId, PaginationDto pagination)
        {
            try
            {
                var notifications = await _unitOfWork.VehicleNotifications.GetByUserAsync(userId);

                var totalCount = notifications.Count();
                var pagedNotifications = notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize);

                var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(pagedNotifications);

                return new PaginatedResult<NotificationDto>
                {
                    Items = notificationDtos,
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<NotificationDto>
                {
                    Items = new List<NotificationDto>(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };
            }
        }

        public async Task<PaginatedResult<NotificationDto>> GetNotificationsByVehicleAsync(int vehicleId, PaginationDto pagination)
        {
            try
            {
                var notifications = await _unitOfWork.VehicleNotifications.GetByVehicleAsync(vehicleId);

                var totalCount = notifications.Count();
                var pagedNotifications = notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize);

                var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(pagedNotifications);

                return new PaginatedResult<NotificationDto>
                {
                    Items = notificationDtos,
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<NotificationDto>
                {
                    Items = new List<NotificationDto>(),
                    TotalCount = 0,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };
            }
        }

        public async Task<Result<bool>> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _unitOfWork.VehicleNotifications.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return Result<bool>.Failure("Bildirim bulunamadı.");
                }

                notification.MarkAsRead();
                await _unitOfWork.VehicleNotifications.UpdateAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Bildirim okundu olarak işaretlenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> MarkAllAsReadByUserAsync(int userId)
        {
            try
            {
                await _unitOfWork.VehicleNotifications.MarkAllAsReadByUserAsync(userId);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Tüm bildirimler okundu olarak işaretlenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteNotificationAsync(int id)
        {
            try
            {
                await _unitOfWork.VehicleNotifications.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Bildirim silinirken hata: {ex.Message}");
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId)
        {
            try
            {
                var notifications = await _unitOfWork.VehicleNotifications.GetUnreadByUserAsync(userId);
                return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            }
            catch (Exception ex)
            {
                return new List<NotificationDto>();
            }
        }

        public async Task<int> GetUnreadCountByUserAsync(int userId)
        {
            try
            {
                return await _unitOfWork.VehicleNotifications.GetUnreadCountByUserAsync(userId);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Result<bool>> SendNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _unitOfWork.VehicleNotifications.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return Result<bool>.Failure("Bildirim bulunamadı.");
                }

                if (notification.IsSent)
                {
                    return Result<bool>.Failure("Bildirim zaten gönderilmiş.");
                }

                // Send email if user has email
                if (notification.UserId.HasValue)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(notification.UserId.Value);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        // Email sending logic would go here
                        // await _emailService.SendEmailAsync(user.Email, notification.Title, notification.Message);
                    }
                }

                notification.MarkAsSent();
                await _unitOfWork.VehicleNotifications.UpdateAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Bildirim gönderilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ScheduleInsuranceExpiryNotificationAsync(int vehicleId, DateTime expiryDate)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                var notification = new VehicleNotification(
                    vehicleId,
                    NotificationType.InsuranceExpiring,
                    "Sigorta Süresi Dolmak Üzere",
                    $"{vehicle.PlateNumber} plakalı aracın sigortası {expiryDate:dd.MM.yyyy} tarihinde sona erecek.",
                    expiryDate.AddDays(-30), // 30 gün önceden uyar
                    vehicle.AssignedUserId,
                    3 // High priority
                );

                await _unitOfWork.VehicleNotifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Sigorta süre sonu bildirimi oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ScheduleMaintenanceReminderAsync(int maintenanceId, DateTime reminderDate)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(maintenanceId);
                if (maintenance == null)
                {
                    return Result<bool>.Failure("Bakım bulunamadı.");
                }

                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(maintenance.VehicleId);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                var notification = new VehicleNotification(
                    maintenance.VehicleId,
                    NotificationType.MaintenanceDue,
                    "Bakım Hatırlatması",
                    $"{vehicle.PlateNumber} plakalı aracın bakımı {maintenance.ScheduledDate:dd.MM.yyyy} tarihinde planlanmıştır.",
                    reminderDate,
                    vehicle.AssignedUserId,
                    2 // Medium priority
                );

                await _unitOfWork.VehicleNotifications.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Bakım hatırlatması oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CreateBulkNotificationsAsync(IEnumerable<CreateNotificationDto> notifications)
        {
            try
            {
                var notificationEntities = new List<VehicleNotification>();

                foreach (var dto in notifications)
                { 
                    var notification = new VehicleNotification(
                        dto.VehicleId,
                        dto.Type,
                        dto.Title,
                        dto.Message,
                        dto.ScheduledDate,
                        dto.UserId,
                        dto.Priority
                    );

                    notificationEntities.Add(notification);
                }

                await _unitOfWork.VehicleNotifications.AddRangeAsync(notificationEntities);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Toplu bildirim oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetPendingNotificationsAsync()
        {
            try
            {
                var notifications = await _unitOfWork.VehicleNotifications.GetPendingNotificationsAsync();
                return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            }
            catch (Exception ex)
            {
                return new List<NotificationDto>();
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetOverdueNotificationsAsync()
        {
            try
            {
                var notifications = await _unitOfWork.VehicleNotifications.GetOverdueNotificationsAsync();
                return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            }
            catch (Exception ex)
            {
                return new List<NotificationDto>();
            }
        }
    }
}