using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleInspectionConfiguration : BaseEntityConfiguration<VehicleInspection>
    {
        public override void Configure(EntityTypeBuilder<VehicleInspection> builder)
        {
            base.Configure(builder);

            builder.ToTable("VehicleInspections");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.InspectionDate)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.InspectionCenter)
                .HasMaxLength(200);

            builder.Property(x => x.CertificateNumber)
                .HasMaxLength(100);

            builder.Property(x => x.VehicleKm)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.ExpiryDate);
            builder.HasIndex(x => x.Status);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.Inspections)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}