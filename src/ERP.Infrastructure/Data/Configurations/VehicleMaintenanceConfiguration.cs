using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleMaintenanceConfiguration : BaseEntityConfiguration<VehicleMaintenance>
    {
        public override void Configure(EntityTypeBuilder<VehicleMaintenance> builder)
        {
            base.Configure(builder);

            builder.ToTable("VehicleMaintenances");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.VehicleKm)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ServiceProvider)
                .HasMaxLength(200);

            builder.Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.NextMaintenanceKm)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.ScheduledDate);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.IsCompleted);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.Maintenances)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}