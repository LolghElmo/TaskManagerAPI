using IdentityService.Application.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdentityService.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Configure MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AutoMapperProfiles).Assembly));

            // Configure AutoMapper
            services.AddAutoMapper(options =>
            {
                options.AddProfile<AutoMapperProfiles>();
            });

            return services;
        }
        public static void AddSerilog(this IHostBuilder host)
        {
            // Configure Serilog

            host.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
