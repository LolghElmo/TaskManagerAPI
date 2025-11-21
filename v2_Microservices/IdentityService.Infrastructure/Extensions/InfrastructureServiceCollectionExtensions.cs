using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Models;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IdentityService.Infrastructure.Extensions
{
    public static  class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Ensure Database Connection String is valid
            var conString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Invalid Connection String 'DefaultConnection' not found");

            // Add DbContext and Identity
            services.AddDbContext<DataContext>(options => {
                options.UseSqlite(conString);
            });
            services.AddIdentity<User, Roles>( options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // Injection
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // Ensure JWT token is valid
            var secretKey = configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JWT Secret Key not found in configuration");

            // Add Authentication with JWT Bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
