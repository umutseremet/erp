using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class SystemLogConfiguration : BaseEntityConfiguration<SystemLog>
    {
        public override void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("SystemLogs");

            builder.Property(x => x.Level)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Source)
                .HasMaxLength(100);

            builder.Property(x => x.UserName)
                .HasMaxLength(100);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.Level);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CreatedAt);

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}