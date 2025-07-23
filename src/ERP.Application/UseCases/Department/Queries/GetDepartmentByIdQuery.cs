using ERP.Application.Common.Models;
using ERP.Application.DTOs.Department;
using MediatR;

namespace ERP.Application.UseCases.Department.Queries
{
    public class GetDepartmentByIdQuery : IRequest<Result<DepartmentDto>>
    {
        public int Id { get; set; }
    }
}