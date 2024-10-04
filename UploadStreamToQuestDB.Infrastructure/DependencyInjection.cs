using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UploadStreamToQuestDB.Infrastructure.Services;

namespace UploadStreamToQuestDB.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructureDependencies
            (this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IQueryIngestionerService>(x =>
            new QueryIngestionerService(
                configuration["QuestDbAddress"],
                configuration["QuestDbPort"],
                configuration["QuestDbSettings"]));

            return services;
        }
    }
}
