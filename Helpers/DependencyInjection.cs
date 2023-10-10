using StorageApp.CloudProvider.Config;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Services;

namespace StorageApp.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudOptions>(configuration.GetSection("Cloud"));
            services.AddScoped<ICloudStorageServiceFactory, CloudStorageServiceFactory>();
            services.AddTransient<ICloudConfiguration, CloudConfigurationService>();

            return services;
        }
    }
}
