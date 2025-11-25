using IdentityService.Application.Extensions;
using IdentityService.Infrastructure.Extensions;

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
