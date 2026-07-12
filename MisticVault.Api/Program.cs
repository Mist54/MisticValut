using Microsoft.EntityFrameworkCore;
using MisticVault.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Use in-memory EF Core for development/testing and register DbContext
//builder.Services.AddDbContext<MisticVault.Infrastructure.Data.MisticVaultDbContext>(options =>
//    options.UseInMemoryDatabase("MisticVault"));

// Use SQL Server for production and register DbContext --> developer/production environment should have a valid connection string in appsettings.json
builder.Services.AddDbContext<MisticVaultDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
// Development-time OpenAPI generation was disabled to avoid source-generator incompatibilities
// with the installed OpenAPI package versions.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
