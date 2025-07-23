using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Department;
using ERP.Application.UseCases.Department.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Department.Handlers
{
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);
            if (department == null)
            {
                return Result<DepartmentDto>.Failure("Departman bulunamadı.");
            }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Result<DepartmentDto>.Success(departmentDto);
        }
    }

    public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsQuery, PaginatedResult<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<DepartmentDto>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();

            // Filtering
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                departments = departments.Where(d =>
                    d.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (d.Code != null && d.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (d.Description != null && d.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (request.IsActive.HasValue)
            {
                departments = departments.Where(d => d.IsActive == request.IsActive.Value);
            }

            if (request.ParentDepartmentId.HasValue)
            {
                departments = departments.Where(d => d.ParentDepartmentId == request.ParentDepartmentId.Value);
            }

            var totalCount = departments.Count();

            // Pagination
            departments = departments
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            return new PaginatedResult<DepartmentDto>
            {
                Items = departmentDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }

    public class GetDepartmentTreeHandler : IRequestHandler<GetDepartmentTreeQuery, IEnumerable<DepartmentTreeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDepartmentTreeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentTreeDto>> Handle(GetDepartmentTreeQuery request, CancellationToken cancellationToken)
        {
            var departments = request.IncludeInactive
                ? await _unitOfWork.Departments.GetAllAsync()
                : await _unitOfWork.Departments.GetActiveAsync();

            var rootDepartments = departments.Where(d => d.ParentDepartmentId == null).ToList();
            var departmentTree = new List<DepartmentTreeDto>();

            foreach (var rootDept in rootDepartments)
            {
                var treeDto = BuildDepartmentTree(rootDept, departments.ToList());
                departmentTree.Add(treeDto);
            }

            return departmentTree;
        }

        private DepartmentTreeDto BuildDepartmentTree(ERP.Core.Entities.Department department, List<ERP.Core.Entities.Department> allDepartments)
        {
            var treeDto = _mapper.Map<DepartmentTreeDto>(department);

            var children = allDepartments.Where(d => d.ParentDepartmentId == department.Id).ToList();
            treeDto.Children = children.Select(child => BuildDepartmentTree(child, allDepartments)).ToList();

            return treeDto;
        }
    }
}