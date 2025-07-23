using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class InsurancePolicyConfiguration : BaseEntityConfiguration<InsurancePolicy>
    {
        public override void Configure(EntityTypeBuilder<InsurancePolicy> builder)
        {
            base.Configure(builder);

            builder.ToTable("InsurancePolicies");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.PolicyNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.InsuranceCompany)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PolicyType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.PremiumAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CoverageAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("TRY");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.PolicyNumber).IsUnique();
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.EndDate);
            builder.HasIndex(x => x.IsActive);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}