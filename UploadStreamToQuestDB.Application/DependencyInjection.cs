using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UploadStreamToQuestDB.Application;
using UploadStreamToQuestDB.Application.Handlers;

namespace UploadStreamToQuestDB.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddApplicationDependencies
            (this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IUploadPipeline, UploadPipeline>();
            services.AddScoped<IUploadHandler, UploadHandler>();
            services.AddScoped<IExtensionHandler, ExtensionHandler>();
            services.AddScoped<IAntivirusHandler, AntivirusHandler>();
            services.AddScoped<IDataIngestionerHandler, DataIngestionerHandler>();
            services.AddScoped<IDiskCleanUpHandler, DiskCleanUpHandler>();

            return services;
        }
    }
}
