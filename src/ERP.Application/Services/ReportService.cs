using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Dashboard;
using ERP.Application.DTOs.Reports;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.Interfaces.Services;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using System.Text;

namespace ERP.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<byte[]>> GenerateVehicleReportAsync(VehicleReportRequestDto request)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

                // Filtreleme
                if (request.Status.HasValue)
                    vehicles = vehicles.Where(v => v.Status == request.Status.Value);

                if (request.Type.HasValue)
                    vehicles = vehicles.Where(v => v.Type == request.Type.Value);

                
                if (request.StartDate.HasValue)
                    vehicles = vehicles.Where(v => v.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    vehicles = vehicles.Where(v => v.CreatedAt <= request.EndDate.Value);

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Plaka,Marka,Model,Yıl,Tip,Durum,Atanan Kullanıcı,Km,Oluşturma Tarihi");

                foreach (var vehicle in vehicles)
                {
                    csv.AppendLine($"{vehicle.PlateNumber},{vehicle.Brand},{vehicle.Model},{vehicle.Year},{vehicle.Type},{vehicle.Status},{vehicle.AssignedUser?.FullName ?? "Atanmamış"},{vehicle.CurrentKm},{vehicle.CreatedAt:dd.MM.yyyy}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Araç raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateMaintenanceReportAsync(MaintenanceReportRequestDto request)
        {
            try
            {
                var maintenances = await _unitOfWork.VehicleMaintenances.GetAllAsync();

                // Filtreleme
                if (request.VehicleId.HasValue)
                    maintenances = maintenances.Where(m => m.VehicleId == request.VehicleId.Value);

                if (request.MaintenanceType.HasValue)
                    maintenances = maintenances.Where(m => m.Type == request.MaintenanceType.Value);

                if (request.StartDate.HasValue)
                    maintenances = maintenances.Where(m => m.ScheduledDate >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    maintenances = maintenances.Where(m => m.ScheduledDate <= request.EndDate.Value);

                if (request.IsCompleted.HasValue)
                    maintenances = maintenances.Where(m => m.IsCompleted == request.IsCompleted.Value);

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Araç Plaka,Bakım Türü,Açıklama,Planlanan Tarih,Tamamlanma Tarihi,Maliyet,Servis Sağlayıcı,Durum");

                foreach (var maintenance in maintenances)
                {
                    csv.AppendLine($"{maintenance.Vehicle.PlateNumber},{maintenance.Type},{maintenance.Description},{maintenance.ScheduledDate:dd.MM.yyyy},{maintenance.CompletedDate?.ToString("dd.MM.yyyy") ?? "Tamamlanmamış"},{maintenance.Cost?.ToString("C") ?? "Belirtilmemiş"},{maintenance.ServiceProvider ?? "Belirtilmemiş"},{(maintenance.IsCompleted ? "Tamamlandı" : "Beklemede")}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Bakım raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateFuelReportAsync(FuelReportRequestDto request)
        {
            try
            {
                var fuelTransactions = await _unitOfWork.FuelTransactions.GetAllAsync();

                // Filtreleme
                if (request.VehicleId.HasValue)
                    fuelTransactions = fuelTransactions.Where(f => f.VehicleId == request.VehicleId.Value);

                if (request.StartDate.HasValue)
                    fuelTransactions = fuelTransactions.Where(f => f.TransactionDate >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    fuelTransactions = fuelTransactions.Where(f => f.TransactionDate <= request.EndDate.Value);

                if (request.FuelType.HasValue)
                    fuelTransactions = fuelTransactions.Where(f => f.FuelType == request.FuelType.Value);

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Araç Plaka,Tarih,Yakıt Türü,Miktar (L),Birim Fiyat,Toplam Tutar,İstasyon,Araç Km");

                foreach (var transaction in fuelTransactions)
                {
                    csv.AppendLine($"{transaction.Vehicle.PlateNumber},{transaction.TransactionDate:dd.MM.yyyy},{transaction.FuelType},{transaction.Quantity},{transaction.UnitPrice:C},{transaction.TotalAmount:C},{transaction.StationName ?? "Belirtilmemiş"},{transaction.VehicleKm}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Yakıt raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateInsuranceReportAsync(InsuranceReportRequestDto request)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetAllAsync();

                // Filtreleme
                if (request.VehicleId.HasValue)
                    policies = policies.Where(p => p.VehicleId == request.VehicleId.Value);

                if (!string.IsNullOrEmpty(request.InsuranceCompany))
                    policies = policies.Where(p => p.InsuranceCompany.Contains(request.InsuranceCompany, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(request.PolicyType))
                    policies = policies.Where(p => p.PolicyType.Contains(request.PolicyType, StringComparison.OrdinalIgnoreCase));

                if (request.StartDate.HasValue)
                    policies = policies.Where(p => p.StartDate >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    policies = policies.Where(p => p.EndDate <= request.EndDate.Value);

                if (request.IsActive.HasValue)
                    policies = policies.Where(p => p.IsActive == request.IsActive.Value);

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Araç Plaka,Poliçe No,Sigorta Şirketi,Poliçe Türü,Başlangıç Tarihi,Bitiş Tarihi,Prim Tutarı,Teminat Tutarı,Durum");

                foreach (var policy in policies)
                {
                    csv.AppendLine($"{policy.Vehicle.PlateNumber},{policy.PolicyNumber},{policy.InsuranceCompany},{policy.PolicyType},{policy.StartDate:dd.MM.yyyy},{policy.EndDate:dd.MM.yyyy},{policy.PremiumAmount:C},{policy.CoverageAmount:C},{(policy.IsActive ? "Aktif" : "Pasif")}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Sigorta raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateUserReportAsync(UserReportRequestDto request)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();

                // Filtreleme
                if (request.UserStatus.HasValue)
                    users = users.Where(u => u.Status == request.UserStatus.Value);

                if (request.DepartmentId.HasValue)
                    users = users.Where(u => u.UserDepartments.Any(ud => ud.DepartmentId == request.DepartmentId.Value && ud.IsActive));

                //if (request.HasVehicleAssignment.HasValue)
                //{
                //    if (request.HasVehicleAssignment.Value)
                //        users = users.Where(u => u.AssignedVehicles.Any(v => v.Status == VehicleStatus.Assigned));
                //    else
                //        users = users.Where(u => !u.AssignedVehicles.Any(v => v.Status == VehicleStatus.Assigned));
                //}

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Ad,Soyad,Email,Telefon,Personel No,Durum,Departman,Atanan Araç,Son Giriş");

                foreach (var user in users)
                {
                    var department = user.UserDepartments.FirstOrDefault(ud => ud.IsPrimary && ud.IsActive)?.Department?.Name ?? "Atanmamış";
                    var assignedVehicle = user.AssignedVehicles.FirstOrDefault(v => v.Status == VehicleStatus.Assigned)?.PlateNumber ?? "Atanmamış";

                    csv.AppendLine($"{user.FirstName},{user.LastName},{user.Email},{user.PhoneNumber ?? "Belirtilmemiş"},{user.EmployeeNumber ?? "Belirtilmemiş"},{user.Status},{department},{assignedVehicle},{user.LastLoginDate?.ToString("dd.MM.yyyy HH:mm") ?? "Hiç giriş yapmamış"}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Kullanıcı raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<byte[]>> GenerateDepartmentReportAsync(DepartmentReportRequestDto request)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();

                if (request.IsActive.HasValue)
                    departments = departments.Where(d => d.IsActive == request.IsActive.Value);

                // CSV formatında rapor oluştur
                var csv = new StringBuilder();
                csv.AppendLine("Departman Adı,Kod,Açıklama,Üst Departman,Kullanıcı Sayısı,Alt Departman Sayısı,Durum");

                foreach (var department in departments)
                {
                    var userCount = department.UserDepartments.Count(ud => ud.IsActive);
                    var subDepartmentCount = department.SubDepartments.Count(sd => sd.IsActive);

                    csv.AppendLine($"{department.Name},{department.Code ?? "Belirtilmemiş"},{department.Description ?? "Belirtilmemiş"},{department.ParentDepartment?.Name ?? "Ana Departman"},{userCount},{subDepartmentCount},{(department.IsActive ? "Aktif" : "Pasif")}");
                }

                return Result<byte[]>.Success(Encoding.UTF8.GetBytes(csv.ToString()));
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Departman raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<DashboardDataDto>> GetDashboardDataAsync(int? userId = null)
        {
            try
            {
                var dashboardData = new DashboardDataDto();

                // Araç istatistikleri
                var vehicles = await _unitOfWork.Vehicles.GetAllAsync();
                //dashboardData.TotalVehicles = vehicles.Count();
                //dashboardData.AvailableVehicles = vehicles.Count(v => v.Status == VehicleStatus.Available);
                //dashboardData.AssignedVehicles = vehicles.Count(v => v.Status == VehicleStatus.Assigned);
                //dashboardData.InMaintenanceVehicles = vehicles.Count(v => v.Status == VehicleStatus.InMaintenance);

                // Kullanıcı istatistikleri
                var users = await _unitOfWork.Users.GetAllAsync();
                //dashboardData.TotalUsers = users.Count();
                //dashboardData.ActiveUsers = users.Count(u => u.Status == UserStatus.Active);

                // Yakıt istatistikleri (son 30 gün)
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                var recentFuelTransactions = await _unitOfWork.FuelTransactions.GetByDateRangeAsync(thirtyDaysAgo, DateTime.UtcNow);
                //dashboardData.MonthlyFuelCost = recentFuelTransactions.Sum(f => f.TotalAmount);

                // Bakım istatistikleri
                var upcomingMaintenances = await _unitOfWork.VehicleMaintenances.GetUpcomingMaintenanceAsync();
                //dashboardData.UpcomingMaintenances = upcomingMaintenances.Count();

                // Sigorta istatistikleri
                var expiringPolicies = await _unitOfWork.InsurancePolicies.GetExpiringPoliciesAsync(30);
                //dashboardData.ExpiringInsurancePolicies = expiringPolicies.Count();

                return Result<DashboardDataDto>.Success(dashboardData);
            }
            catch (Exception ex)
            {
                return Result<DashboardDataDto>.Failure($"Dashboard verileri getirilirken hata oluştu: {ex.Message}");
            }
        }

        // Placeholder implementations for remaining methods
        public Task<Result<byte[]>> GenerateExecutiveSummaryAsync(ExecutiveSummaryRequestDto request) => throw new NotImplementedException();
        public Task<Result<byte[]>> GenerateCustomReportAsync(CustomReportRequestDto request) => throw new NotImplementedException();
        public Task<Result<byte[]>> GenerateFleetPerformanceReportAsync(FleetPerformanceRequestDto request) => throw new NotImplementedException();
        public Task<Result<byte[]>> GenerateCostAnalysisReportAsync(CostAnalysisRequestDto request) => throw new NotImplementedException();
        public Task<Result<string[]>> GetAvailableReportTemplatesAsync() => throw new NotImplementedException();
        public Task<Result<byte[]>> CreateReportTemplateAsync(CreateReportTemplateDto request) => throw new NotImplementedException();
        public Task<Result<bool>> DeleteReportTemplateAsync(int templateId) => throw new NotImplementedException();
        public Task<Result<byte[]>> GetKPIDashboardAsync(DateTime startDate, DateTime endDate) => throw new NotImplementedException();
        public Task<Result<byte[]>> ScheduleReportAsync(ScheduledReportDto request) => throw new NotImplementedException();
        public Task<Result<IEnumerable<ScheduledReportDto>>> GetScheduledReportsAsync() => throw new NotImplementedException();
    }
}