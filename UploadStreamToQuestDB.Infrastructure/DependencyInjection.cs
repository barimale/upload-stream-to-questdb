using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UploadStreamToQuestDB.Infrastructure.Services;

namespace UploadStreamToQuestDB.Infrastructure {
    /// <summary>
    /// Provides extension methods for adding infrastructure dependencies.
    /// </summary>
    public static class DependencyInjection {
        /// <summary>
        /// Adds infrastructure dependencies to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
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
