using Microsoft.Extensions.Options;
using Sensix.Lib.Repository;
using Sensix.Lib.Service;

namespace Sensix.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Business Logic
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<IMeasurementService, MeasurementService>();

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IMeasurementRepository, MeasurementRepository>();
    }

    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(
                    "http://localhost:3000", // Docker
                    "http://localhost:5173" // Vite Dev
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
    }
}