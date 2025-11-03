using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using TaskManagerAPI.Data;
using TaskManagerAPI.DbInitializer;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Extensions
{

    public static class ApplicationServerExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            var conString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String 'DefaultConnection' not found");
            services.AddDbContext<DataContext>(options => options.UseSqlite(conString));

            //Configuration Session Options
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IDbInitializer, DbInitializer.DbInitializer>();
            services.AddControllers();
            services.AddCors();

            // Services
            services.AddScoped<ITokenService, TokenService>();

            // Ensure JWT token is valid
            var secretKey = config["AppSettings:TokenKey"] ?? throw new ArgumentNullException("JWT Secret Token is missing from the configuration.");
            // Configure the JWT Authentication
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
