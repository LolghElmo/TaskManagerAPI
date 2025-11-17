using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Application.Mappers;

namespace TaskManagerAPI.Application
{
    public static class ApplicationServerExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(AutoMapperProfiles).Assembly));
            services.AddAutoMapper(options =>
            {
                options.AddProfile<AutoMapperProfiles>();
            });
            return services;
        }
    }
}
