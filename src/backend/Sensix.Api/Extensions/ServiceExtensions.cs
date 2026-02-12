using Sensix.Lib.Database;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;
using Sensix.Lib.Service;

namespace Sensix.Api.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Dependency Injection for Services, Repositories & AutoMapper
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Auto Mapper
        services.AddAutoMapper(cfg => { }, typeof(AppDbContext).Assembly);

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

    /// <summary>
    /// Set allowed urls
    /// </summary>
    /// <param name="services"></param>
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