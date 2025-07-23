using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.UseCases.Fuel.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Handlers
{
    public class UpdateFuelTransactionHandler : IRequestHandler<UpdateFuelTransactionCommand, Result<FuelTransactionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateFuelTransactionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<FuelTransactionDto>> Handle(UpdateFuelTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = await _unitOfWork.FuelTransactions.GetByIdAsync(request.Id);
                if (transaction == null)
                    return Result<FuelTransactionDto>.Failure("Yakıt işlemi bulunamadı");

                transaction.SetQuantityAndPrice(request.Quantity, request.UnitPrice);
                transaction.SetStationInfo(request.StationName, request.StationAddress);

                if (!string.IsNullOrEmpty(request.Notes))
                    transaction.SetNotes(request.Notes);

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Yakıt işlemi güncellenirken hata: {ex.Message}");
            }
        }
    }
}