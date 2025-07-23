using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class VehicleNotificationConfiguration : BaseEntityConfiguration<VehicleNotification>
    {
        public override void Configure(EntityTypeBuilder<VehicleNotification> builder)
        {
            base.Configure(builder);

            builder.ToTable("VehicleNotifications");

            builder.Property(x => x.VehicleId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IsSent)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.ActionUrl)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(x => x.VehicleId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.ScheduledDate);
            builder.HasIndex(x => x.IsRead);
            builder.HasIndex(x => x.IsSent);
            builder.HasIndex(x => x.Priority);

            // Relationships
            builder.HasOne(x => x.Vehicle)
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}