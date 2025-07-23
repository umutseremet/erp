using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.Fuel.Commands;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Handlers
{
    public class CreateFuelTransactionHandler : IRequestHandler<CreateFuelTransactionCommand, Result<FuelTransactionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CreateFuelTransactionHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result<FuelTransactionDto>> Handle(CreateFuelTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
                if (vehicle == null)
                    return Result<FuelTransactionDto>.Failure("Araç bulunamadı");

                var transaction = new FuelTransaction(
                    request.VehicleId,
                    request.TransactionDate,
                    request.Quantity,
                    request.UnitPrice,
                    request.FuelType,
                    request.VehicleKm,
                    request.FuelCardId,
                    request.StationName
                );

                if (!string.IsNullOrEmpty(request.StationAddress))
                    transaction.SetStationInfo(request.StationName, request.StationAddress);

                if (!string.IsNullOrEmpty(request.Notes))
                    transaction.SetNotes(request.Notes);

                await _unitOfWork.FuelTransactions.AddAsync(transaction);
                vehicle.UpdateKilometer(request.VehicleKm);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<FuelTransactionDto>(transaction);
                return Result<FuelTransactionDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<FuelTransactionDto>.Failure($"Yakıt işlemi oluşturulurken hata: {ex.Message}");
            }
        }
    }
}