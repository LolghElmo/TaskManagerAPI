using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Application.Mappers;

namespace TaskManagerAPI.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
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
