using Microsoft.OpenApi.Models;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;
using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract;
using StorageApp.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace StorageApp.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudOptions>(configuration.GetSection("Cloud"));
            services.Configure<RDBMSOptions>(configuration.GetSection("RDBMS"));
            services.AddScoped<ICloudStorageServiceFactory, CloudStorageServiceFactory>();
            services.AddScoped<IRDBMSBuilderFactory,RDBMSBuilderFactory>();
            services.AddTransient<ICloudConfiguration, CloudConfigurationService>();
            services.AddTransient<IRDBMSBuilder, AZUREBuilder>();
            return services;
        }
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1.0", new OpenApiInfo { Title = "CAAF", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
