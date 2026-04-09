using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Interfaces;
using TasksService.Infrastructure.Data;
using TasksService.Infrastructure.Repositories;

namespace TasksService.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<DataContext>(options => 
            { 
                options.UseSqlite(connectionString);
            });

            // Injection
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            // Ensure JWT settings are valid
            var secretKey = configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JWT Secret Key not found in configuration");
            var issuer = configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer not found in configuration");
            var audience = configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT Audience not found in configuration");


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
