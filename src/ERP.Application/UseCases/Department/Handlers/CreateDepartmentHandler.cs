using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Department;
using ERP.Application.UseCases.Department.Commands;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Department.Handlers
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            // Kod kontrolü
            if (!string.IsNullOrEmpty(request.Code))
            {
                var codeExists = await _unitOfWork.Departments.IsCodeExistsAsync(request.Code);
                if (codeExists)
                {
                    return Result<DepartmentDto>.Failure("Bu departman kodu zaten kullanılıyor.");
                }
            }

            // Parent department kontrolü
            if (request.ParentDepartmentId.HasValue)
            {
                var parentDepartment = await _unitOfWork.Departments.GetByIdAsync(request.ParentDepartmentId.Value);
                if (parentDepartment == null)
                {
                    return Result<DepartmentDto>.Failure("Üst departman bulunamadı.");
                }
            }

            var department = new ERP.Core.Entities.Department(
                request.Name,
                request.Description,
                request.Code,
                request.ParentDepartmentId
            );

            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }
}