using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Vehicle.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class GetVehiclesByUserHandler : IRequestHandler<GetVehiclesByUserQuery, Result<IEnumerable<VehicleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVehiclesByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<VehicleDto>>> Handle(GetVehiclesByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetVehiclesByUserAsync(request.UserId);
                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Kullanıcı araçları getirilirken hata: {ex.Message}");
            }
        }
    }
}