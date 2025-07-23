using ERP.Application.DTOs.Department;
using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Department.Commands
{
    public class CreateDepartmentCommand : IRequest<Result<DepartmentDto>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
    }
}