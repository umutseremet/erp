namespace ERP.Application.DTOs.User
{
    public class UserStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int UsersWithVehicles { get; set; }
        public Dictionary<string, int> UsersByDepartment { get; set; } = new();
        public Dictionary<string, int> UsersByRole { get; set; } = new();
    }
}
