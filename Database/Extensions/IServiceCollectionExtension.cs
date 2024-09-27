using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions {
    public static class IServiceCollectionExtension {
        public static IServiceCollection AddSQLDatabase(this IServiceCollection services, IConfiguration Configuration) {
            services.AddDbContext<FileDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
