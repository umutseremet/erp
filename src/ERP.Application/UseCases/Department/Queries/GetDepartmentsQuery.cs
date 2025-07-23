using ERP.Application.DTOs.Department;
using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Department.Queries
{
    public class GetDepartmentsQuery : IRequest<PaginatedResult<DepartmentDto>>
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}