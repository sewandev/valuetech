using Microsoft.Extensions.DependencyInjection;
using ValueTech.Data.Repositories;
using ValueTech.Data.Interfaces;

namespace ValueTech.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IRegionRepository>(provider => 
            {
                var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RegionRepository>>();
                return new RegionRepository(connectionString, logger);
            });
            
            services.AddScoped<IComunaRepository>(provider => 
            {
                var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ComunaRepository>>();
                return new ComunaRepository(connectionString, logger);
            });

            services.AddScoped<IAuthRepository>(provider => 
            {
                return new AuthRepository(connectionString);
            });

            services.AddScoped<IAuditoriaRepository>(provider => 
            {
                return new AuditoriaRepository(connectionString);
            });

            return services;
        }
    }
}
