using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class FuelTransactionConfiguration : BaseEntityConfiguration<FuelTransaction>
    {
        public override void Configure(EntityTypeBuilder<FuelTransaction> builder)
        {
            base.Configure(builder);

            builder.ToTable("FuelTransactions");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.FuelType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.StationName)
                .HasMaxLength(200);

            builder.Property(x => x.StationAddress)
                .HasMaxLength(500);

            builder.Property(x => x.VehicleKm)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.FuelCardId);
            builder.HasIndex(x => x.TransactionDate);
            builder.HasIndex(x => x.FuelType);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.FuelTransactions)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.FuelCard)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.FuelCardId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}