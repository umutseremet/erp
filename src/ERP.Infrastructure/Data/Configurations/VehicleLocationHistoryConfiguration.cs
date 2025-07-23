using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleLocationHistoryConfiguration : BaseEntityConfiguration<VehicleLocationHistory>
    {
        public override void Configure(EntityTypeBuilder<VehicleLocationHistory> builder)
        {
            base.Configure(builder);

            builder.ToTable("VehicleLocationHistories");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.Latitude)
                .IsRequired()
                .HasColumnType("decimal(18,10)");

            builder.Property(x => x.Longitude)
                .IsRequired()
                .HasColumnType("decimal(18,10)");

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            builder.Property(x => x.Speed)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Direction)
                .HasMaxLength(10);

            builder.Property(x => x.Altitude)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Accuracy)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DataSource)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.RecordedAt);
            builder.HasIndex(x => new { x.Latitude, x.Longitude });

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.LocationHistory)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}