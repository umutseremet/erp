using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.UseCases.Fuel.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Handlers
{
    public class GetFuelTransactionsByVehicleHandler : IRequestHandler<GetFuelTransactionsByVehicleQuery, Result<IEnumerable<FuelTransactionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFuelTransactionsByVehicleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<FuelTransactionDto>>> Handle(GetFuelTransactionsByVehicleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var transactions = await _unitOfWork.FuelTransactions.GetByVehicleAsync(request.VehicleId);

                if (request.StartDate.HasValue || request.EndDate.HasValue)
                {
                    var startDate = request.StartDate ?? DateTime.MinValue;
                    var endDate = request.EndDate ?? DateTime.MaxValue;
                    transactions = transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
                }

                var dtos = _mapper.Map<IEnumerable<FuelTransactionDto>>(transactions);
                return Result<IEnumerable<FuelTransactionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FuelTransactionDto>>.Failure($"Yakıt işlemleri getirilirken hata: {ex.Message}");
            }
        }
    }
}