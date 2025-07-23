using Microsoft.EntityFrameworkCore.Storage;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IVehicleRepository Vehicles => GetRepository<IVehicleRepository>(() => new VehicleRepository(_context));
        public IUserRepository Users => GetRepository<IUserRepository>(() => new UserRepository(_context));
        public IDepartmentRepository Departments => GetRepository<IDepartmentRepository>(() => new DepartmentRepository(_context));
        public IFuelTransactionRepository FuelTransactions => GetRepository<IFuelTransactionRepository>(() => new FuelTransactionRepository(_context));
        public IVehicleMaintenanceRepository VehicleMaintenances => GetRepository<IVehicleMaintenanceRepository>(() => new VehicleMaintenanceRepository(_context));
        public IPermissionRepository Permissions => GetRepository<IPermissionRepository>(() => new PermissionRepository(_context));
        public IRoleRepository Roles => GetRepository<IRoleRepository>(() => new RoleRepository(_context));
        public IInsurancePolicyRepository InsurancePolicies => GetRepository<IInsurancePolicyRepository>(() => new InsurancePolicyRepository(_context));
        public IVehicleLocationHistoryRepository VehicleLocationHistories => GetRepository<IVehicleLocationHistoryRepository>(() => new VehicleLocationHistoryRepository(_context));
        public IVehicleNotificationRepository VehicleNotifications => GetRepository<IVehicleNotificationRepository>(() => new VehicleNotificationRepository(_context));

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            return GetRepository<IRepository<T>>(() => new BaseRepository<T>(_context));
        }

        private TRepo GetRepository<TRepo>(Func<TRepo> factory) where TRepo : class
        {
            var type = typeof(TRepo);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = factory();
            }
            return (TRepo)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}