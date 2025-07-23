using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Data.Seed
{
    public static class SeedExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Database'i oluþtur
            await context.Database.EnsureCreatedAsync();

            // Seed data'yý ekle
            var seeder = new DataSeeder(context);
            await seeder.SeedAsync();
        }
    }
}