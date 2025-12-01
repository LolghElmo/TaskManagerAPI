using Scalar.AspNetCore;
using Serilog;
using TasksService.Application.Extensions;
using TasksService.Domain.Interfaces;
using TasksService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Serilog
builder.Host.AddSerilog();

// Add Application services
builder.Services.AddApplicationServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Initializing Database
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
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
