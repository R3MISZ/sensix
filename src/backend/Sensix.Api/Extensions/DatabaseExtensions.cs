using Microsoft.EntityFrameworkCore;
using Sensix.Lib.Database;

namespace Sensix.Api.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Add database & connection string
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    }

    /// <summary>
    /// Applies EF-Core migrations to database
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrated successfully");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}