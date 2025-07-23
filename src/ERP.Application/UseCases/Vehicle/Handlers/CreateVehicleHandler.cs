using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class CreateVehicleHandler : IRequestHandler<CreateVehicleCommand, Result<VehicleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateVehicleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<VehicleDto>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validations
                var plateExists = await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(request.PlateNumber);
                if (plateExists)
                    return Result<VehicleDto>.Failure("Plaka numarası zaten kullanımda");

                var vinExists = await _unitOfWork.Vehicles.IsVinNumberExistsAsync(request.VinNumber);
                if (vinExists)
                    return Result<VehicleDto>.Failure("VIN numarası zaten kullanımda");

                var vehicle = new Core.Entities.Vehicle(
                    request.PlateNumber,
                    request.VinNumber,
                    request.Brand,
                    request.Model,
                    request.Year,
                    request.Type,
                    request.FuelType,
                    request.PurchaseDate,
                    request.PurchasePrice
                );

                vehicle.UpdateKilometer(request.CurrentKm);
                vehicle.SetFuelCapacity(request.FuelCapacity);
                vehicle.SetEngineSize(request.EngineSize);

                if (!string.IsNullOrEmpty(request.Color))
                    vehicle.SetColor(request.Color);

                if (!string.IsNullOrEmpty(request.Notes))
                    vehicle.SetNotes(request.Notes);

                await _unitOfWork.Vehicles.AddAsync(vehicle);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<VehicleDto>(vehicle);
                return Result<VehicleDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<VehicleDto>.Failure($"Araç oluşturulurken hata: {ex.Message}");
            }
        }
    }
}