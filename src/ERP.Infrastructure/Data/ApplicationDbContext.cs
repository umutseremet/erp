using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Infrastructure.Data.Configurations;
using ERP.Infrastructure.Data.Interceptors;

namespace ERP.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<FuelCard> FuelCards { get; set; } = null!;
        public DbSet<FuelTransaction> FuelTransactions { get; set; } = null!;
        public DbSet<InsurancePolicy> InsurancePolicies { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<SystemLog> SystemLogs { get; set; } = null!;
        public DbSet<TireChange> TireChanges { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserDepartment> UserDepartments { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<VehicleInspection> VehicleInspections { get; set; } = null!;
        public DbSet<VehicleLicense> VehicleLicenses { get; set; } = null!;
        public DbSet<VehicleLocationHistory> VehicleLocationHistories { get; set; } = null!;
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; } = null!;
        public DbSet<VehicleNotification> VehicleNotifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new FuelCardConfiguration());
            modelBuilder.ApplyConfiguration(new FuelTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new InsurancePolicyConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new SystemLogConfiguration());
            modelBuilder.ApplyConfiguration(new TireChangeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserDepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleInspectionConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleLicenseConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleLocationHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleMaintenanceConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleNotificationConfiguration());

            // Global query filters for soft delete
            modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<FuelCard>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<FuelTransaction>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<InsurancePolicy>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Permission>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<SystemLog>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<TireChange>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Vehicle>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<VehicleInspection>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<VehicleLicense>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<VehicleLocationHistory>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<VehicleMaintenance>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<VehicleNotification>().HasQueryFilter(e => !e.IsDeleted);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditInterceptor(), new SoftDeleteInterceptor());
        }
    }
}