using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class FuelCardConfiguration : BaseEntityConfiguration<FuelCard>
    {
        public override void Configure(EntityTypeBuilder<FuelCard> builder)
        {
            base.Configure(builder);

            builder.ToTable("FuelCards");

            builder.Property(x => x.CardNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ProviderName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ExpiryDate)
                .IsRequired();

            builder.Property(x => x.CreditLimit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CurrentBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.CardNumber).IsUnique();
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.AssignedVehicleId);
            builder.HasIndex(x => x.ExpiryDate);

            // Relationships
            builder.HasOne(x => x.AssignedVehicle)
                .WithMany()
                .HasForeignKey(x => x.AssignedVehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.FuelCard)
                .HasForeignKey(x => x.FuelCardId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}