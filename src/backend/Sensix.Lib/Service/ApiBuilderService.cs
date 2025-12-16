using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensix.Lib.Database;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public static class ApiBuilderService
{
    public static IServiceCollection AddLibServices(this IServiceCollection services)
    {
        // Interface to class
        services.AddScoped<IDeviceService, DeviceService>();

        services.AddScoped<IDbRepository, DbRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();

        return services;
    }

    public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        // Database connection
        var connectionString = configuration.GetConnectionString("SensixDb");

        services.AddDbContext<SensixDbContext>(options => { options.UseNpgsql(connectionString); });

        return services;
    }
}