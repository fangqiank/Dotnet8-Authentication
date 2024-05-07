using Dotnet8Authentication.Database;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8Authentication.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }
    }
}
