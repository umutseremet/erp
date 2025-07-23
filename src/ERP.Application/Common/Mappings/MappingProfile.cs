// Common/Mappings/MappingProfile.cs
using AutoMapper;
using ERP.Application.Common.Extensions;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Department;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.DTOs.Insurance;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.DTOs.Notification;
using ERP.Application.DTOs.User;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.UseCases.Department.Commands;
using ERP.Application.UseCases.Fuel.Commands;
using ERP.Application.UseCases.Insurance.Commands;
using ERP.Application.UseCases.Maintenance.Commands;
using ERP.Application.UseCases.User.Commands;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Entities;
using ERP.Core.ValueObjects;

namespace ERP.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseEntity, BaseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.RedmineIssueId, opt => opt.MapFrom(src => src.RedmineIssueId))
                .IncludeAllDerived();

            // Vehicle Mappings
            CreateVehicleMappings();

            // User Mappings
            CreateUserMappings();

            // Department Mappings
            CreateDepartmentMappings();

            // Fuel Transaction Mappings
            CreateFuelTransactionMappings();

            // Maintenance Mappings
            CreateMaintenanceMappings();

            // Insurance Mappings
            CreateInsuranceMappings();

            // Notification Mappings
            CreateNotificationMappings();

            // Value Object Mappings
            CreateValueObjectMappings();
        }

        private void CreateVehicleMappings()
        {
            // Entity to DTO
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.PlateNumber))
                .ForMember(dest => dest.VinNumber, opt => opt.MapFrom(src => src.VinNumber))
                .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : null))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<Vehicle, VehicleListDto>()
                .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.PlateNumber))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                //.ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.ToDisplayName()))
                .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : null))
                .ForMember(dest => dest.CurrentKm, opt => opt.MapFrom(src => src.CurrentKm))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<Vehicle, VehicleDetailDto>()
                .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.PlateNumber))
                .ForMember(dest => dest.VinNumber, opt => opt.MapFrom(src => src.VinNumber))
                //.ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.ToDisplayName()))
                //.ForMember(dest => dest.FuelTypeDisplayName, opt => opt.MapFrom(src => src.FuelType.ToDisplayName()))
                //.ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.ToDisplayName()))
                //.ForMember(dest => dest.AssignedUser, opt => opt.MapFrom(src => src.AssignedUser))
                //.ForMember(dest => dest.LastMaintenanceDate, opt => opt.MapFrom(src => src.Maintenances.OrderByDescending(m => m.CompletedDate).FirstOrDefault()!.CompletedDate))
                //.ForMember(dest => dest.NextMaintenanceDate, opt => opt.MapFrom(src => src.Maintenances.OrderByDescending(m => m.NextMaintenanceDate).FirstOrDefault()!.NextMaintenanceDate))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateVehicleCommand, Vehicle>()
                .ForMember(dest => dest.PlateNumber, opt => opt.MapFrom(src => src.PlateNumber))
                .ForMember(dest => dest.VinNumber, opt => opt.MapFrom(src => src.VinNumber))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore())
                .ForMember(dest => dest.Maintenances, opt => opt.Ignore())
                .ForMember(dest => dest.Inspections, opt => opt.Ignore())
                .ForMember(dest => dest.Licenses, opt => opt.Ignore())
                .ForMember(dest => dest.LocationHistory, opt => opt.Ignore())
                .ForMember(dest => dest.FuelTransactions, opt => opt.Ignore())
                .ForMember(dest => dest.TireChanges, opt => opt.Ignore());

            CreateMap<CreateVehicleDto, CreateVehicleCommand>();
            CreateMap<UpdateVehicleDto, UpdateVehicleCommand>();
        }

        private void CreateUserMappings()
        {
            // Entity to DTO
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.UserDepartments.Where(ud => ud.IsActive).Select(ud => ud.Department!.Name)))
                //.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role!.Name)))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                //.ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.ToDisplayName()))
                //.ForMember(dest => dest.PrimaryDepartment, opt => opt.MapFrom(src => src.UserDepartments.Where(ud => ud.IsActive && ud.IsPrimary).Select(ud => ud.Department!.Name).FirstOrDefault()))
                .ForMember(dest => dest.AssignedVehicleCount, opt => opt.MapFrom(src => src.AssignedVehicles.Count))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<User, UserProfileDto>()
                //.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                //.ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src => src.Status.ToDisplayName()))
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.UserDepartments.Where(ud => ud.IsActive).Select(ud => new UserDepartmentDto
                {
                    DepartmentId = ud.DepartmentId,
                    DepartmentName = ud.Department!.Name,
                    IsPrimary = ud.IsPrimary,
                    AssignedDate = ud.AssignedDate
                })))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Where(ur => ur.IsActive).Select(ur => new UserRoleDto
                {
                    RoleId = ur.RoleId,
                    RoleName = ur.Role!.Name,
                    AssignedDate = ur.AssignedDate,
                    ExpiryDate = ur.ExpiryDate
                })))
                .ForMember(dest => dest.AssignedVehicles, opt => opt.MapFrom(src => src.AssignedVehicles))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UserDepartments, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedVehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Notifications, opt => opt.Ignore());

            CreateMap<CreateUserDto, CreateUserCommand>();
            CreateMap<UpdateUserDto, UpdateUserCommand>();
        }

        private void CreateDepartmentMappings()
        {
            // Entity to DTO
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.ParentDepartmentName, opt => opt.MapFrom(src => src.ParentDepartment != null ? src.ParentDepartment.Name : null))
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.UserDepartments.Count(ud => ud.IsActive)))
                //.ForMember(dest => dest.SubDepartmentCount, opt => opt.MapFrom(src => src.SubDepartments.Count(sd => sd.IsActive)))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<Department, DepartmentTreeDto>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.SubDepartments.Where(sd => sd.IsActive)))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateDepartmentCommand, Department>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ParentDepartment, opt => opt.Ignore())
                .ForMember(dest => dest.SubDepartments, opt => opt.Ignore())
                .ForMember(dest => dest.UserDepartments, opt => opt.Ignore());

            CreateMap<CreateDepartmentDto, CreateDepartmentCommand>();
            CreateMap<UpdateDepartmentDto, UpdateDepartmentCommand>();
        }

        private void CreateFuelTransactionMappings()
        {
            // Entity to DTO
            CreateMap<FuelTransaction, FuelTransactionDto>()
                .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                .ForMember(dest => dest.FuelCardNumber, opt => opt.MapFrom(src => src.FuelCard != null ? src.FuelCard.CardNumber : null))
                //.ForMember(dest => dest.FuelTypeDisplayName, opt => opt.MapFrom(src => src.FuelType.ToDisplayName()))
                //.ForMember(dest => dest.TotalAmountFormatted, opt => opt.MapFrom(src => src.TotalAmount.ToCurrencyString()))
                //.ForMember(dest => dest.QuantityFormatted, opt => opt.MapFrom(src => src.Quantity.ToFuelQuantityString()))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<FuelTransaction, FuelTransactionListDto>()
                .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                //.ForMember(dest => dest.FuelTypeDisplayName, opt => opt.MapFrom(src => src.FuelType.ToDisplayName()))
                .ForMember(dest => dest.StationName, opt => opt.MapFrom(src => src.StationName))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateFuelTransactionCommand, FuelTransaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Calculated in constructor
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
                .ForMember(dest => dest.FuelCard, opt => opt.Ignore());

            CreateMap<CreateFuelTransactionDto, CreateFuelTransactionCommand>();
        }

        private void CreateMaintenanceMappings()
        {
            // Entity to DTO
            CreateMap<VehicleMaintenance, VehicleMaintenanceDto>()
                //.ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                //.ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.ToDisplayName()))
                //.ForMember(dest => dest.CostFormatted, opt => opt.MapFrom(src => src.Cost.HasValue ? src.Cost.Value.ToCurrencyString() : "-"))
                //.ForMember(dest => dest.VehicleKmFormatted, opt => opt.MapFrom(src => src.VehicleKm.ToKilometerString()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .IncludeBase<BaseEntity, BaseDto>();

            //CreateMap<VehicleMaintenance, MaintenanceReportDto>()
            //    .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
            //    .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.Brand))
            //    .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
            //    .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.ToDisplayName()))
            //    .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateMaintenanceCommand, VehicleMaintenance>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

            CreateMap<CreateMaintenanceDto, CreateMaintenanceCommand>();
            //CreateMap<UpdateMaintenanceDto, UpdateMaintenanceCommand>();
        }

        private void CreateInsuranceMappings()
        {
            // Entity to DTO
            CreateMap<InsurancePolicy, InsurancePolicyDto>()
                .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                //.ForMember(dest => dest.PremiumAmountFormatted, opt => opt.MapFrom(src => src.PremiumAmount.ToCurrencyString(src.Currency)))
                //.ForMember(dest => dest.CoverageAmountFormatted, opt => opt.MapFrom(src => src.CoverageAmount.ToCurrencyString(src.Currency)))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.IsExpiringSoon, opt => opt.MapFrom(src => src.IsExpiringSoon))
                .ForMember(dest => dest.DaysUntilExpiry, opt => opt.MapFrom(src => src.DaysUntilExpiry))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            CreateMap<CreateInsurancePolicyCommand, InsurancePolicy>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicle, opt => opt.Ignore());

            CreateMap<CreateInsurancePolicyDto, CreateInsurancePolicyCommand>();
            CreateMap<UpdateInsurancePolicyDto, UpdateInsurancePolicyCommand>();
        }

        private void CreateNotificationMappings()
        {
            // Entity to DTO
            CreateMap<VehicleNotification, NotificationDto>()
                .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                //.ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.ToDisplayName()))
                //.ForMember(dest => dest.PriorityDisplayName, opt => opt.MapFrom(src => src.GetPriorityText()))
                //.ForMember(dest => dest.IsDue, opt => opt.MapFrom(src => src.IsDue))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .IncludeBase<BaseEntity, BaseDto>();

            CreateMap<VehicleNotification, NotificationSummaryDto>()
                //.ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
                //.ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.Type.ToDisplayName()))
                .IncludeBase<BaseEntity, BaseDto>();

            // Command to Entity
            //CreateMap<CreateNotificationCommand, VehicleNotification>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            //    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            //    .ForMember(dest => dest.IsSent, opt => opt.Ignore())
            //    .ForMember(dest => dest.IsRead, opt => opt.Ignore())
            //    .ForMember(dest => dest.SentDate, opt => opt.Ignore())
            //    .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
            //    .ForMember(dest => dest.User, opt => opt.Ignore());

            //CreateMap<CreateNotificationDto, CreateNotificationCommand>();
        }

        private void CreateValueObjectMappings()
        {
            // Value Objects to string
            CreateMap<PlateNumber, string>().ConvertUsing(src => src.Value);
            CreateMap<VinNumber, string>().ConvertUsing(src => src.Value);
            CreateMap<PhoneNumber, string>().ConvertUsing(src => src.Value);
            CreateMap<Money, decimal>().ConvertUsing(src => src.Amount);

            // string to Value Objects
            CreateMap<string, PlateNumber>().ConvertUsing(src => new PlateNumber(src));
            CreateMap<string, VinNumber>().ConvertUsing(src => new VinNumber(src));
            CreateMap<string, PhoneNumber>().ConvertUsing(src => new PhoneNumber(src));
        }
    }
}