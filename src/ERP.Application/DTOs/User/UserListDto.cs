using ERP.Application.DTOs.Common;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.User
{
    public class UserListDto : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? EmployeeNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public int AssignedVehicleCount { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
     
    /// <summary>
    /// Kullanıcının rol bilgileri için DTO
    /// </summary>
    public class UserRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsExpired => ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value;
        public bool IsValid => IsActive && !IsExpired;
        public string StatusText => IsValid ? "Geçerli" : (IsExpired ? "Süresi Dolmuş" : "Pasif");
        public string ExpiryText => ExpiryDate?.ToString("dd.MM.yyyy") ?? "Süresiz";
    }

    /// <summary>
    /// Kullanıcıya atanan araç bilgileri için DTO
    /// </summary>
    public class AssignedVehicleDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string VehicleInfo => $"{Year} {Brand} {Model}";
        public VehicleStatus Status { get; set; }
        public string StatusText => Status switch
        {
            VehicleStatus.Available => "Müsait",
            VehicleStatus.Assigned => "Atanmış",
            VehicleStatus.InMaintenance => "Bakımda",
            VehicleStatus.OutOfService => "Hizmet Dışı",
            VehicleStatus.Inspection => "Muayenede",
            VehicleStatus.Accident => "Kaza",
            _ => Status.ToString()
        };
        public DateTime AssignedDate { get; set; }
        public string FormattedAssignedDate => AssignedDate.ToString("dd.MM.yyyy");
    }
}