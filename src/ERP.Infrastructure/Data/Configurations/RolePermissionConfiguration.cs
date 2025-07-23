using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
    {
        public override void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            base.Configure(builder);

            builder.ToTable("RolePermissions");

            builder.Property(x => x.RoleId)
                .IsRequired();

            builder.Property(x => x.PermissionId)
                .IsRequired();

            // Composite index for unique constraint
            builder.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();

            // Relationships
            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}