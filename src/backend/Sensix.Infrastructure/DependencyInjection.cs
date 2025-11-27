using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sensix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("SensixDb");

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'SensixDb' is not configured.");

        services.AddDbContext<SensixDbContext>(options =>
        {
            options.UseNpgsql(cs);
        });

        return services;
    }
}
