using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityService.Application.Mappers;
using IdentityService.Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdentityService.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddFluentValidationAutoValidation();



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
