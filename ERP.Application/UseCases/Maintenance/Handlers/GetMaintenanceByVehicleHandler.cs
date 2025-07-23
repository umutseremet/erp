using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.UseCases.Maintenance.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Handlers
{
    public class GetMaintenanceByVehicleHandler : IRequestHandler<GetMaintenanceByVehicleQuery, Result<IEnumerable<MaintenanceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMaintenanceByVehicleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MaintenanceDto>>> Handle(GetMaintenanceByVehicleQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                return Result<IEnumerable<MaintenanceDto>>.Failure("Araç bulunamadı.");
            }

            var maintenances = await _unitOfWork.VehicleMaintenances.GetByVehicleAsync(request.VehicleId);

            if (!request.IncludeCompleted)
            {
                maintenances = maintenances.Where(m => !m.IsCompleted);
            }

            if (request.StartDate.HasValue)
            {
                maintenances = maintenances.Where(m => m.ScheduledDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                maintenances = maintenances.Where(m => m.ScheduledDate <= request.EndDate.Value);
            }

            var maintenanceDtos = _mapper.Map<IEnumerable<MaintenanceDto>>(maintenances);
            return Result<IEnumerable<MaintenanceDto>>.Success(maintenanceDtos);
        }
    }
}