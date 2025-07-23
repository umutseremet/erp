using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Vehicle.Queries;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class GetAvailableVehiclesHandler : IRequestHandler<GetAvailableVehiclesQuery, Result<IEnumerable<VehicleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableVehiclesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<VehicleDto>>> Handle(GetAvailableVehiclesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = await _unitOfWork.Vehicles.GetAvailableVehiclesAsync();

                if (!request.IncludeAssigned)
                    vehicles = vehicles.Where(v => v.Status == VehicleStatus.Available);

                if (request.Type.HasValue)
                    vehicles = vehicles.Where(v => v.Type == request.Type.Value);

                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDto>>(vehicles.OrderBy(v => v.PlateNumber));
                return Result<IEnumerable<VehicleDto>>.Success(vehicleDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<VehicleDto>>.Failure($"Müsait araçlar getirilirken hata: {ex.Message}");
            }
        }
    }
}