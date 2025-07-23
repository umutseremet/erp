using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureGlobalQueryFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ModelBuilderExtensions)
                        .GetMethod(nameof(SetSoftDeleteQueryFilter))
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, [modelBuilder]);
                }
            }
        }

        public static void SetSoftDeleteQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}