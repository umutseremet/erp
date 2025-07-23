using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Vehicle.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class GetVehiclesHandler : IRequestHandler<GetVehiclesQuery, PaginatedResult<VehicleListDto>>
    {
        private readonly IVehicleRepository _vehicleRepository; // Change to specific repository
        private readonly IMapper _mapper;

        public GetVehiclesHandler(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<VehicleListDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            // Filtering
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                vehicles = vehicles.Where(v =>
                    v.PlateNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    v.Brand.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    v.Model.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (request.Status.HasValue)
            {
                vehicles = vehicles.Where(v => v.Status == request.Status.Value);
            }

            if (request.VehicleType.HasValue)
            {
                vehicles = vehicles.Where(v => v.Type == request.VehicleType.Value);
            }

            if (request.UserId.HasValue)
            {
                vehicles = vehicles.Where(v => v.AssignedUserId == request.UserId.Value);
            }

            if (request.DepartmentId.HasValue)
            {
                // This would need to be implemented based on user-department relationship
                // vehicles = vehicles.Where(v => v.AssignedUser.UserDepartments.Any(ud => ud.DepartmentId == request.DepartmentId.Value));
            }

            var totalCount = vehicles.Count();

            // Pagination
            vehicles = vehicles
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var vehicleDtos = _mapper.Map<IEnumerable<VehicleListDto>>(vehicles);

            return new PaginatedResult<VehicleListDto>
            {
                Items = (List<VehicleListDto>)vehicleDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}