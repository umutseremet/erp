using ERP.Core.Enums;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Queries
{
    public class GetVehiclesQuery : IRequest<PaginatedResult<VehicleListDto>>
    {
        public string? SearchTerm { get; set; }
        public VehicleStatus? Status { get; set; }
        public VehicleType? VehicleType { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserId { get; set; }
        public int? Year { get; set; }
        public string? Brand { get; set; }
        public bool? HasActiveInsurance { get; set; }
        public bool? NeedsMaintenance { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}