using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleLicenseConfiguration : BaseEntityConfiguration<VehicleLicense>
    {
        public override void Configure(EntityTypeBuilder<VehicleLicense> builder)
        {
            base.Configure(builder);

            builder.ToTable("VehicleLicenses");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.LicenseNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LicenseType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IssueDate)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.IssuingAuthority)
                .HasMaxLength(200);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.LicenseNumber);
            builder.HasIndex(x => x.ExpiryDate);
            builder.HasIndex(x => x.IsActive);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.Licenses)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}