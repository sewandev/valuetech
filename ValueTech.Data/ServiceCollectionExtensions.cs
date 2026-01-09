using Microsoft.Extensions.DependencyInjection;
using ValueTech.Data.Repositories;

namespace ValueTech.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IRegionRepository>(provider => new RegionRepository(connectionString));
            services.AddScoped<IComunaRepository>(provider => new ComunaRepository(connectionString));

            return services;
        }
    }
}
