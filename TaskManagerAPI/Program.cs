using Serilog;
using TaskManagerAPI.DbInitializer;
using TaskManagerAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
builder.Host.SerilogConfiguration();

// Add services to the container.
builder.Services.AddApplicationService(builder.Configuration);

// Enable AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatic HTTP Request Logging with Serilog
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors(builder=> builder
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:3000"));
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
