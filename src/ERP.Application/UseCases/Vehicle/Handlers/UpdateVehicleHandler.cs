using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Exceptions;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class UpdateVehicleHandler : IRequestHandler<UpdateVehicleCommand, Result<VehicleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateVehicleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<VehicleDto>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException(request.Id);
            }

            // Plaka numarası değişikliği kontrolü
            if (vehicle.PlateNumber != request.PlateNumber)
            {
                var existingVehicle = await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(request.PlateNumber, request.Id);
                if (existingVehicle)
                {
                    return Result<VehicleDto>.Failure("Bu plaka numarası zaten kullanılıyor.");
                }
            }

            // VIN numarası değişikliği kontrolü
            if (vehicle.VinNumber != request.VinNumber)
            {
                var existingVin = await _unitOfWork.Vehicles.IsVinNumberExistsAsync(request.VinNumber, request.Id);
                if (existingVin)
                {
                    return Result<VehicleDto>.Failure("Bu VIN numarası zaten kullanılıyor.");
                }
            }

            // Vehicle güncelleme
            vehicle.SetPlateNumber(request.PlateNumber);
            vehicle.SetVinNumber(request.VinNumber);
            vehicle.SetBrandModel(request.Brand, request.Model);
            vehicle.SetYear(request.Year);

            if (request.CurrentKm > vehicle.CurrentKm)
            {
                vehicle.UpdateKilometer(request.CurrentKm);
            }

            if (!string.IsNullOrEmpty(request.Color))
            {
                // Color property'si için setter eklenmeli
                // vehicle.SetColor(request.Color);
            }

            if (!string.IsNullOrEmpty(request.Notes))
            {
                vehicle.SetNotes(request.Notes);
            }

            await _unitOfWork.Vehicles.UpdateAsync(vehicle);
            await _unitOfWork.SaveChangesAsync();

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return Result<VehicleDto>.Success(vehicleDto);
        }
    }
}