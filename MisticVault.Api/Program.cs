using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Use in-memory EF Core for development/testing and register DbContext
builder.Services.AddDbContext<MisticVault.Infrastructure.Data.MisticVaultDbContext>(options =>
    options.UseInMemoryDatabase("MisticVault"));

// Register application services
builder.Services.AddScoped<MisticVault.Core.Todo.Services.ITodoService, MisticVault.Infrastructure.Todo.Services.TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Development-time OpenAPI generation was disabled to avoid source-generator incompatibilities
// with the installed OpenAPI package versions.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
