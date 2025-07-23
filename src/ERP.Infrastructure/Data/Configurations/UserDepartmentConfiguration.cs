using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class UserDepartmentConfiguration : BaseEntityConfiguration<UserDepartment>
    {
        public override void Configure(EntityTypeBuilder<UserDepartment> builder)
        {
            base.Configure(builder);

            builder.ToTable("UserDepartments");

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.DepartmentId)
                .IsRequired();

            builder.Property(x => x.AssignedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.DepartmentId);
            builder.HasIndex(x => x.IsActive);

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserDepartments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.UserDepartments)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}