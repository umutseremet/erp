using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class TireChangeConfiguration : BaseEntityConfiguration<TireChange>
    {
        public override void Configure(EntityTypeBuilder<TireChange> builder)
        {
            base.Configure(builder);

            builder.ToTable("TireChanges");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.ChangeDate)
                .IsRequired();

            builder.Property(x => x.VehicleKm)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TireBrand)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TireSize)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.ServiceProvider)
                .HasMaxLength(200);

            builder.Property(x => x.Cost)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.ChangeDate);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany(x => x.TireChanges)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}