using ERP.Application.DTOs.Department;
using MediatR;

namespace ERP.Application.UseCases.Department.Queries
{
    public class GetDepartmentTreeQuery : IRequest<IEnumerable<DepartmentTreeDto>>
    {
        public bool IncludeInactive { get; set; } = false;
    }
}