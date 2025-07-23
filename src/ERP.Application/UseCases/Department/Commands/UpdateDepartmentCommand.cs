using ERP.Application.Common.Models;
using ERP.Application.DTOs.Department;
using MediatR;

namespace ERP.Application.UseCases.Department.Commands
{
    public class UpdateDepartmentCommand : IRequest<Result<DepartmentDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}