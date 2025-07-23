namespace ERP.Application.Common.Constants
{
    public static class CacheKeys
    {
        public const string VEHICLES = "vehicles";
        public const string USERS = "users";
        public const string DEPARTMENTS = "departments";
        public const string ACTIVE_VEHICLES = "active_vehicles";
        public const string DASHBOARD_DATA = "dashboard_data";
        public const string VEHICLE_STATISTICS = "vehicle_statistics";

        public static string VehicleById(int id) => $"vehicle_{id}";
        public static string UserById(int id) => $"user_{id}";
        public static string DepartmentById(int id) => $"department_{id}";
        public static string VehiclesByUser(int userId) => $"vehicles_user_{userId}";
        public static string UsersByDepartment(int deptId) => $"users_dept_{deptId}";
    }
}
