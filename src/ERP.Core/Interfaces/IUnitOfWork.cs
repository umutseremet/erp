using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRepository Vehicles { get; }
        IUserRepository Users { get; }
        IDepartmentRepository Departments { get; }
        IFuelTransactionRepository FuelTransactions { get; }
        IVehicleMaintenanceRepository VehicleMaintenances { get; }
        IPermissionRepository Permissions { get; }
        IRoleRepository Roles { get; }
        IInsurancePolicyRepository InsurancePolicies { get; }
        IVehicleLocationHistoryRepository VehicleLocationHistories { get; }
        IVehicleNotificationRepository VehicleNotifications { get; }
        
        IRepository<T> Repository<T>() where T : BaseEntity;
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}