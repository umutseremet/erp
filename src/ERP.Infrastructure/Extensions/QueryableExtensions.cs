using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;

namespace ERP.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query) where T : BaseEntity
        {
            return query.IgnoreQueryFilters();
        }

        public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) where T : BaseEntity
        {
            return query.IgnoreQueryFilters().Where(e => e.IsDeleted);
        }

        public static async Task<IEnumerable<T>> ToListWithPaginationAsync<T>(this IQueryable<T> query, int page, int size)
        {
            return await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public static IQueryable<T> OrderByCreatedDate<T>(this IQueryable<T> query, bool ascending = true) where T : BaseEntity
        {
            return ascending ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);
        }

        public static IQueryable<T> OrderByUpdatedDate<T>(this IQueryable<T> query, bool ascending = true) where T : BaseEntity
        {
            return ascending ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);
        }
    }
}