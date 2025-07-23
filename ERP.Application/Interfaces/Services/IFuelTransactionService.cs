// 3. IFuelTransactionService Interface
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.Common.Models;

namespace ERP.Application.Interfaces.Services
{
    public interface IFuelTransactionService
    {
        Task<Result<FuelTransactionDto>> GetFuelTransactionByIdAsync(int id);
        Task<PaginatedResult<FuelTransactionDto>> GetFuelTransactionsPagedAsync(FuelTransactionFilterDto filter, int pageNumber, int pageSize);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByVehicleAsync(int vehicleId);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByCardAsync(int fuelCardId);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<Result<FuelTransactionDto>> CreateFuelTransactionAsync(CreateFuelTransactionDto dto);
        Task<Result<FuelTransactionDto>> UpdateFuelTransactionAsync(int id, UpdateFuelTransactionDto dto);
        Task<Result<bool>> DeleteFuelTransactionAsync(int id);
        Task<Result<decimal>> GetTotalFuelCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<decimal>> GetTotalFuelQuantityByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<FuelTransactionDto>> GetLastTransactionByVehicleAsync(int vehicleId);
        Task<Result<FuelEfficiencyDto>> GetFuelEfficiencyAnalysisAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetMonthlyFuelSummaryAsync(int? vehicleId = null, int? year = null, int? month = null);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelCostReportAsync(FuelReportFilterDto filter);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelEfficiencyReportAsync(FuelReportFilterDto filter);
        Task<Result<bool>> ValidateKilometerConsistencyAsync(int vehicleId, decimal newKm);
        Task<Result<bool>> ValidateFuelConsumptionAsync(CreateFuelTransactionDto dto);
        Task<Result<bool>> CheckFuelCardBalanceAsync(int fuelCardId, decimal amount);
        Task<Result<bool>> UpdateFuelCardBalanceAsync(int fuelCardId, decimal amount, string? description = null);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetAbnormalConsumptionAlertAsync(int? vehicleId = null);
        Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelCardBalanceAlertAsync();
        Task<Result<IEnumerable<FuelTransactionDto>>> ExportFuelTransactionsAsync(FuelExportFilterDto filter);
        Task<Result<bool>> ImportFuelTransactionsAsync(byte[] data, FuelImportFormat format);
        Task<Result<string>> GetImportTemplateAsync(FuelImportFormat format);
        Task<Result<IEnumerable<CreateFuelTransactionDto>>> CreateBulkFuelTransactionsAsync(IEnumerable<CreateFuelTransactionDto> transactions);
    }
}