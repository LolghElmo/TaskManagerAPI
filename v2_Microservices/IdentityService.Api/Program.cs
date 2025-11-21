using IdentityService.Application.Extensions;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add Infrastructure Services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Application Services
builder.Services.AddApplicationServices(builder.Configuration);

// Configure Serilog
builder.Host.AddSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
}

app.UseHttpsRedirection();

// Use Session
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
