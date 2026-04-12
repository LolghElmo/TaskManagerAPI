using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TasksService.Application.Mappers;
using TasksService.Application.Validators;

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

            // Configure FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();
            services.AddFluentValidationAutoValidation();

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
