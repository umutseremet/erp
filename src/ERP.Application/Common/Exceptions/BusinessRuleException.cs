// 3. Common/Exceptions/BusinessRuleException.cs
namespace ERP.Application.Common.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException() : base("İş kuralı ihlali")
        {
        }

        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BusinessRuleException(string ruleName, string message)
            : base($"İş kuralı ihlali ({ruleName}): {message}")
        {
            RuleName = ruleName;
        }

        public string? RuleName { get; }

        // Common business rule factory methods
        public static BusinessRuleException VehicleNotAvailable(string plateNumber)
        {
            return new BusinessRuleException("VehicleAvailability",
                $"Araç ({plateNumber}) şu anda kullanılamaz durumda");
        }

        public static BusinessRuleException InsufficientFuelCardBalance(decimal balance, decimal requiredAmount)
        {
            return new BusinessRuleException("FuelCardBalance",
                $"Yakıt kartı bakiyesi yetersiz. Mevcut: {balance:C}, Gerekli: {requiredAmount:C}");
        }

        public static BusinessRuleException UserAlreadyAssignedToVehicle(string userName, string plateNumber)
        {
            return new BusinessRuleException("UserVehicleAssignment",
                $"Kullanıcı ({userName}) zaten bir araca ({plateNumber}) atanmış");
        }

        public static BusinessRuleException VehicleAlreadyAssigned(string plateNumber, string userName)
        {
            return new BusinessRuleException("VehicleAssignment",
                $"Araç ({plateNumber}) zaten başka bir kullanıcıya ({userName}) atanmış");
        }

        public static BusinessRuleException MaintenanceAlreadyCompleted(int maintenanceId)
        {
            return new BusinessRuleException("MaintenanceCompletion",
                $"Bakım ({maintenanceId}) zaten tamamlanmış");
        }

        public static BusinessRuleException InvalidKilometer(decimal currentKm, decimal newKm)
        {
            return new BusinessRuleException("KilometerValidation",
                $"Kilometre geriye gidemez. Mevcut: {currentKm}, Yeni: {newKm}");
        }

        public static BusinessRuleException ExpiredInsurancePolicy(string policyNumber, DateTime expiryDate)
        {
            return new BusinessRuleException("InsurancePolicy",
                $"Sigorta poliçesi ({policyNumber}) süresi dolmuş: {expiryDate:dd.MM.yyyy}");
        }

        public static BusinessRuleException DuplicateEmail(string email)
        {
            return new BusinessRuleException("EmailUniqueness",
                $"Email adresi ({email}) zaten kullanılıyor");
        }

        public static BusinessRuleException DuplicatePlateNumber(string plateNumber)
        {
            return new BusinessRuleException("PlateNumberUniqueness",
                $"Plaka numarası ({plateNumber}) zaten kayıtlı");
        }

        public static BusinessRuleException InvalidDateRange(DateTime startDate, DateTime endDate)
        {
            return new BusinessRuleException("DateRange",
                $"Geçersiz tarih aralığı: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
        }

        public static BusinessRuleException FuelCardExpired(string cardNumber, DateTime expiryDate)
        {
            return new BusinessRuleException("FuelCardExpiry",
                $"Yakıt kartı ({cardNumber}) süresi dolmuş: {expiryDate:dd.MM.yyyy}");
        }

        public static BusinessRuleException UserNotActive(string userName)
        {
            return new BusinessRuleException("UserStatus",
                $"Kullanıcı ({userName}) aktif değil");
        }

        public static BusinessRuleException CannotDeleteSystemRole(string roleName)
        {
            return new BusinessRuleException("SystemRole",
                $"Sistem rolü ({roleName}) silinemez");
        }

        public static BusinessRuleException CannotDeleteSystemPermission(string permissionName)
        {
            return new BusinessRuleException("SystemPermission",
                $"Sistem yetkisi ({permissionName}) silinemez");
        }
    }
}