using IdentityService.Api.Middleware;
using IdentityService.Application.Extensions;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.AddSerilog();

// Add Application Services
builder.Services.AddApplication(builder.Configuration);

// Add Infrastructure Services
builder.Services.AddInfrastructure(builder.Configuration);



// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors();

// Global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Build the app
var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Middleware for global exception handling
app.UseExceptionHandler();



app.UseSerilogRequestLogging();

// app.UseHttpsRedirection()
app.UseCors(builder => builder
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000"));
// Use Session
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Expose Program class for integration tests
public partial class Program { }
