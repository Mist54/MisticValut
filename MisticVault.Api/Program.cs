using Microsoft.EntityFrameworkCore;
using MisticVault.Infrastructure.Data;
using AutoMapper;

// <summary>
// The main entry point of the application.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Use in-memory EF Core for development/testing and register DbContext
//builder.Services.AddDbContext<MisticVault.Infrastructure.Data.MisticVaultDbContext>(options =>
//    options.UseInMemoryDatabase("MisticVault"));

// Use SQL Server for production and register DbContext --> developer/production environment should have a valid connection string in appsettings.json
// For testing environment, default to in-memory provider (can be overridden by tests)
if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<MisticVaultDbContext>(options => options.UseInMemoryDatabase("IntegrationTestDb"));
}
else
{
    builder.Services.AddDbContext<MisticVaultDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
// Register application repositories and services
builder.Services.AddScoped<MisticVault.Application.Todo.Interfaces.ITodoRepository, MisticVault.Infrastructure.Repositories.TodoRepository>();
builder.Services.AddScoped<MisticVault.Application.Todo.Interfaces.ITodoService, MisticVault.Infrastructure.Services.TodoService>();
// Register AutoMapper profiles from Application assembly
builder.Services.AddAutoMapper(typeof(MisticVault.Application.Todo.Mappings.TodoProfile).Assembly);

// Add Swagger/OpenAPI support
var app = builder.Build();

// Configure the HTTP request pipeline.
// Development-time OpenAPI generation was disabled to avoid source-generator incompatibilities
// with the installed OpenAPI package versions.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Partial Program class required for WebApplicationFactory in integration tests
public partial class Program { }
