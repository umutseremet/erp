using ERP.Core.Enums;

namespace ERP.Application.DTOs.User
{
    public class UpdateUserBulkDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public int? DepartmentId { get; set; }
    }
}
