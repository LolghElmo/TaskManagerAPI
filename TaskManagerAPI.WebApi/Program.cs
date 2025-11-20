using Scalar.AspNetCore;
using Serilog;
using TaskManagerAPI.Application.Extensions;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region Builder Start up Configurations

// Serilog Configuration
builder.Host.ConfigureSerilog();

// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Application Services
builder.Services.AddApplicationServices();

#endregion 

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Add Endpoints API Explorer for Scaler
builder.Services.AddEndpointsApiExplorer();

// Configure Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors();


var app = builder.Build();

// Run database migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbInitializer = services.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddPreferredSecuritySchemes("Bearer");
        options.AddHttpAuthentication("BearerAuth", auth =>
        {
            auth.Token = null;
        });

        options.EnablePersistentAuthentication();
    });
}

// Automatic HTTP Request Logging with Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors(builder => builder
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000"));

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
