using DotNet.Testcontainers.Builders;
using IdentityService.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.MsSql;
namespace IdentityService.IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Good_password_123!")
            .Build();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    s => s.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor is not null) services.Remove(descriptor);

                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(_dbContainer.GetConnectionString()));
            });

        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _dbContainer.GetConnectionString());
            Environment.SetEnvironmentVariable("JwtSettings__SecretKey", "super-secret-test-key-that-is-long-enough-1234567890");
            Environment.SetEnvironmentVariable("JwtSettings__Issuer", "test-issuer");
            Environment.SetEnvironmentVariable("JwtSettings__Audience", "test-audience");
        }
        public new async Task DisposeAsync() => await _dbContainer.StopAsync();

    }
}
