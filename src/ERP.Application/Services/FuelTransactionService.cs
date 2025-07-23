// 1. FuelTransactionService Implementation
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Core.Interfaces;
using ERP.Core.Entities;
using ERP.Core.Enums;
using AutoMapper;

namespace ERP.Application.Services
{
    public class FuelTransactionService : IFuelTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly INotificationService _notificationService;

        public FuelTransactionService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _notificationService = notificationService;
        }

        public async Task<Result<FuelTransactionDto>> GetFuelTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _unitOfWork.FuelTransactions.GetByIdAsync(id);
                if (transaction == null)
                {
                    return Result<FuelTransactionDto>.Failure("Yakıt işlemi bulunamadı.");
                }

                var transactionDto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(transactionDto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Yakıt işlemi getirilirken hata: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<FuelTransactionDto>> GetFuelTransactionsPagedAsync(FuelTransactionFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetAllAsync();

                // Apply filters
                if (filter.VehicleId.HasValue)
                {
                    transactions = transactions.Where(t => t.VehicleId == filter.VehicleId.Value);
                }

                if (filter.FuelCardId.HasValue)
                {
                    transactions = transactions.Where(t => t.FuelCardId == filter.FuelCardId.Value);
                }

                if (filter.StartDate.HasValue)
                {
                    transactions = transactions.Where(t => t.TransactionDate >= filter.StartDate.Value);
                }

                if (filter.EndDate.HasValue)
                {
                    transactions = transactions.Where(t => t.TransactionDate <= filter.EndDate.Value);
                }

                if (filter.FuelType.HasValue)
                {
                    transactions = transactions.Where(t => t.FuelType == filter.FuelType.Value);
                }

                if (!string.IsNullOrEmpty(filter.StationName))
                {
                    transactions = transactions.Where(t => t.StationName != null &&
                        t.StationName.Contains(filter.StationName, StringComparison.OrdinalIgnoreCase));
                }

                var totalCount = transactions.Count();
                var pagedTransactions = transactions
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var transactionDtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(pagedTransactions);

                return new PaginatedResult<FuelTransactionDto>
                {
                    Items = transactionDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<FuelTransactionDto>
                {
                    Items = new List<FuelTransactionDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByVehicleAsync(int vehicleId)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetByVehicleAsync(vehicleId);
                var transactionDtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(transactions);
                return Result<IEnumerable<FuelTransactionDto>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FuelTransactionDto>>.Failure($"Araç yakıt işlemleri getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByCardAsync(int fuelCardId)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetByFuelCardAsync(fuelCardId);
                var transactionDtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(transactions);
                return Result<IEnumerable<FuelTransactionDto>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FuelTransactionDto>>.Failure($"Yakıt kartı işlemleri getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetByDateRangeAsync(startDate, endDate);
                var transactionDtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(transactions);
                return Result<IEnumerable<FuelTransactionDto>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FuelTransactionDto>>.Failure($"Tarih aralığı yakıt işlemleri getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelTransactionsByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetByVehicleAndDateRangeAsync(vehicleId, startDate, endDate);
                var transactionDtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(transactions);
                return Result<IEnumerable<FuelTransactionDto>>.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FuelTransactionDto>>.Failure($"Araç ve tarih aralığı yakıt işlemleri getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<FuelTransactionDto>> CreateFuelTransactionAsync(CreateFuelTransactionDto dto)
        {
            try
            {
                // Vehicle validation
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                {
                    return Result<FuelTransactionDto>.Failure("Araç bulunamadı.");
                }

                // Fuel card validation if provided
                if (dto.FuelCardId.HasValue)
                {
                    var fuelCard = await _unitOfWork.Repository<FuelCard>().GetByIdAsync(dto.FuelCardId.Value);
                    if (fuelCard == null || !fuelCard.IsActive)
                    {
                        return Result<FuelTransactionDto>.Failure("Yakıt kartı bulunamadı veya aktif değil.");
                    }

                    // Check fuel card balance
                    var hasBalance = await CheckFuelCardBalanceAsync(dto.FuelCardId.Value, dto.Quantity * dto.UnitPrice);
                    if (!hasBalance.IsSuccess || !hasBalance.Data)
                    {
                        return Result<FuelTransactionDto>.Failure("Yakıt kartı bakiyesi yetersiz.");
                    }
                }

                // Validate kilometer consistency
                var kmValidation = await ValidateKilometerConsistencyAsync(dto.VehicleId, dto.VehicleKm);
                if (!kmValidation.IsSuccess || !kmValidation.Data)
                {
                    return Result<FuelTransactionDto>.Failure("Kilometre değeri tutarsız.");
                }

                var transaction = new FuelTransaction(
                    dto.VehicleId,
                    dto.TransactionDate,
                    dto.Quantity,
                    dto.UnitPrice,
                    dto.FuelType,
                    dto.VehicleKm,
                    dto.FuelCardId,
                    dto.StationName
                );

                if (!string.IsNullOrEmpty(dto.StationAddress))
                {
                    transaction.SetStationInfo(dto.StationName, dto.StationAddress);
                }

                if (!string.IsNullOrEmpty(dto.Notes))
                {
                    transaction.SetNotes(dto.Notes);
                }

                await _unitOfWork.FuelTransactions.AddAsync(transaction);

                // Update vehicle kilometer
                vehicle.UpdateKilometer(dto.VehicleKm);
                await _unitOfWork.Vehicles.UpdateAsync(vehicle);

                // Update fuel card balance if used
                if (dto.FuelCardId.HasValue)
                {
                    await UpdateFuelCardBalanceAsync(dto.FuelCardId.Value, -transaction.TotalAmount, "Yakıt alımı");
                }

                await _unitOfWork.SaveChangesAsync();

                var transactionDto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(transactionDto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Yakıt işlemi oluşturulurken hata: {ex.Message}");
            }
        }

        public async Task<Result<FuelTransactionDto>> UpdateFuelTransactionAsync(int id, UpdateFuelTransactionDto dto)
        {
            try
            {
                var transaction = await _unitOfWork.FuelTransactions.GetByIdAsync(id);
                if (transaction == null)
                {
                    return Result<FuelTransactionDto>.Failure("Yakıt işlemi bulunamadı.");
                }

                transaction.SetQuantityAndPrice(dto.Quantity, dto.UnitPrice);
                transaction.SetStationInfo(dto.StationName, dto.StationAddress);
                transaction.SetNotes(dto.Notes);

                await _unitOfWork.FuelTransactions.UpdateAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                var transactionDto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(transactionDto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Yakıt işlemi güncellenirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteFuelTransactionAsync(int id)
        {
            try
            {
                await _unitOfWork.FuelTransactions.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Yakıt işlemi silinirken hata: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetTotalFuelCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var totalCost = await _unitOfWork.FuelTransactions.GetTotalFuelCostByVehicleAsync(vehicleId, startDate, endDate);
                return Result<decimal>.Success(totalCost);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Toplam yakıt maliyeti hesaplanırken hata: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetTotalFuelQuantityByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var totalQuantity = await _unitOfWork.FuelTransactions.GetTotalFuelQuantityByVehicleAsync(vehicleId, startDate, endDate);
                return Result<decimal>.Success(totalQuantity);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Toplam yakıt miktarı hesaplanırken hata: {ex.Message}");
            }
        }

        public async Task<Result<FuelTransactionDto>> GetLastTransactionByVehicleAsync(int vehicleId)
        {
            try
            {
                var transaction = await _unitOfWork.FuelTransactions.GetLastTransactionByVehicleAsync(vehicleId);
                if (transaction == null)
                {
                    return Result<FuelTransactionDto>.Failure("Bu araç için yakıt işlemi bulunamadı.");
                }

                var transactionDto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(transactionDto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Son yakıt işlemi getirilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ValidateKilometerConsistencyAsync(int vehicleId, decimal newKm)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return Result<bool>.Failure("Araç bulunamadı.");
                }

                // New kilometer should not be less than current
                if (newKm < vehicle.CurrentKm)
                {
                    return Result<bool>.Success(false);
                }

                // Check if the increase is reasonable (not more than 1000km since last transaction)
                var lastTransaction = await _unitOfWork.FuelTransactions.GetLastTransactionByVehicleAsync(vehicleId);
                if (lastTransaction != null)
                {
                    var kmDifference = newKm - lastTransaction.VehicleKm;
                    if (kmDifference > 1000) // Configurable threshold
                    {
                        return Result<bool>.Success(false);
                    }
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kilometre tutarlılığı kontrol edilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CheckFuelCardBalanceAsync(int fuelCardId, decimal amount)
        {
            try
            {
                var fuelCard = await _unitOfWork.Repository<FuelCard>().GetByIdAsync(fuelCardId);
                if (fuelCard == null)
                {
                    return Result<bool>.Failure("Yakıt kartı bulunamadı.");
                }

                return Result<bool>.Success(fuelCard.HasSufficientBalance(amount));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Yakıt kartı bakiyesi kontrol edilirken hata: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateFuelCardBalanceAsync(int fuelCardId, decimal amount, string? description = null)
        {
            try
            {
                var fuelCard = await _unitOfWork.Repository<FuelCard>().GetByIdAsync(fuelCardId);
                if (fuelCard == null)
                {
                    return Result<bool>.Failure("Yakıt kartı bulunamadı.");
                }

                fuelCard.UpdateBalance(amount);
                await _unitOfWork.Repository<FuelCard>().UpdateAsync(fuelCard);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Yakıt kartı bakiyesi güncellenirken hata: {ex.Message}");
            }
        }

        // Placeholder implementations for remaining methods
        public async Task<Result<FuelEfficiencyDto>> GetFuelEfficiencyAnalysisAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return Result<FuelEfficiencyDto>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetMonthlyFuelSummaryAsync(int? vehicleId = null, int? year = null, int? month = null)
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelCostReportAsync(FuelReportFilterDto filter)
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelEfficiencyReportAsync(FuelReportFilterDto filter)
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ValidateFuelConsumptionAsync(CreateFuelTransactionDto dto)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetAbnormalConsumptionAlertAsync(int? vehicleId = null)
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> GetFuelCardBalanceAlertAsync()
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> ExportFuelTransactionsAsync(FuelExportFilterDto filter)
        {
            return Result<IEnumerable<FuelTransactionDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ImportFuelTransactionsAsync(byte[] data, FuelImportFormat format)
        {
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<string>> GetImportTemplateAsync(FuelImportFormat format)
        {
            return Result<string>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<CreateFuelTransactionDto>>> CreateBulkFuelTransactionsAsync(IEnumerable<CreateFuelTransactionDto> transactions)
        {
            return Result<IEnumerable<CreateFuelTransactionDto>>.Failure("Method not implemented yet");
        }
    }
}