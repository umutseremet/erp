using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Department;
using ERP.Application.UseCases.Department.Commands;
using ERP.Core.Exceptions;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Department.Handlers
{
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, Result<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);
            if (department == null)
            {
                throw new DepartmentNotFoundException(request.Id);
            }

            // Kod değişikliği kontrolü
            if (!string.IsNullOrEmpty(request.Code) && department.Code != request.Code)
            {
                var codeExists = await _unitOfWork.Departments.IsCodeExistsAsync(request.Code, request.Id);
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

                // Kendi kendisine parent olamaz
                if (request.ParentDepartmentId == request.Id)
                {
                    return Result<DepartmentDto>.Failure("Departman kendi kendisinin üst departmanı olamaz.");
                }
            }

            department.SetName(request.Name);
            department.SetDescription(request.Description);
            department.SetCode(request.Code);

            if (request.IsActive)
                department.Activate();
            else
                department.Deactivate();

            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }
}