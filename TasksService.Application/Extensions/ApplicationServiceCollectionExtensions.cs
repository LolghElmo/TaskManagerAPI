using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.Mappers;

namespace TasksService.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AutoMapperProfile).Assembly));

            // Configure AutoMapper
            services.AddAutoMapper(options =>
            {
                options.AddProfile<AutoMapperProfile>();
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
