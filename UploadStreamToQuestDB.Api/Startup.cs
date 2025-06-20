using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questdb.Net;
using Serilog;
using System;
using UploadStreamToQuestDB.API.Middlewares.GlobalExceptions.Handler;
using UploadStreamToQuestDB.API.SwaggerFilters;
using UploadStreamToQuestDB.Application;
using UploadStreamToQuestDB.Infrastructure;

namespace UploadStreamToQuestDB.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddProblemDetails(options =>
                   options.CustomizeProblemDetails = ctx =>
                       ctx.ProblemDetails.Extensions.Add("nodeId", Environment.MachineName));
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddControllers();
            services.AddQuestDb(Configuration["ReadQuestDbAddress"]);
            services.AddInfrastructureDependencies(Configuration);
            services.AddApplicationDependencies(Configuration);

            services.Configure<FormOptions>(x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddSwaggerGen(options => {
                options.EnableAnnotations();
                options.OperationFilter<AddCustomHeaderParameter>();
                options.OperationFilter<FileUploadOperation>();
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseExceptionHandler();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseHsts();
            app.UseHttpsRedirection();

            var builder = WebApplication.CreateBuilder();
            builder.Host.UseSerilog();
        }
    }
}
