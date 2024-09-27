using Database;
using File.Api.SwaggerFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.SwaggerFilters;
using System;
using Infrastructure.Extensions;

namespace File.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddSQLDatabase(Configuration);

            services.Configure<FormOptions>(x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = Int64.MaxValue; // In case of multipart
            });

            services.AddScoped<IRepository, Repository>();

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.OperationFilter<AddCustomHeaderParameter>();
                options.OperationFilter<FileUploadOperation>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
