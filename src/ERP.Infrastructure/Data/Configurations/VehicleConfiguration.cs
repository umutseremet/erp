using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;
using ERP.Infrastructure.Converters;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleConfiguration : BaseEntityConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            base.Configure(builder);

            builder.ToTable("Vehicles");

            builder.Property(x => x.PlateNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<PlateNumberConverter>();

            builder.Property(x => x.VinNumber)
                .IsRequired()
                .HasMaxLength(17)
                .HasConversion<VinNumberConverter>();

            builder.Property(x => x.Brand)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.Color)
                .HasMaxLength(50);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.FuelType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.CurrentKm)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.FuelCapacity)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.EngineSize)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PurchaseDate)
                .IsRequired();

            builder.Property(x => x.PurchasePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.PlateNumber).IsUnique();
            builder.HasIndex(x => x.VinNumber).IsUnique();
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.AssignedUserId);

            // Relationships
            builder.HasOne(x => x.AssignedUser)
                .WithMany(x => x.AssignedVehicles)
                .HasForeignKey(x => x.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Maintenances)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Inspections)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Licenses)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.LocationHistory)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.FuelTransactions)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.TireChanges)
                .WithOne(x => x.Vehicle)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}