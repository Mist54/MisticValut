using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MisticVault.Api;
using MisticVault.Infrastructure.Data;
using MisticVault.Application.Todo.DTOs.Todo;
using Xunit;

namespace MisticVault.Tests
{
    public class TodoControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TodoControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostAndGetTodo_Works()
        {
            var dbName = Guid.NewGuid().ToString(); // Unique DB per test

            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext registrations
                    var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<MisticVaultDbContext>) || d.ServiceType == typeof(MisticVaultDbContext)).ToList();
                    foreach (var d in descriptors) services.Remove(d);

                    // Register in-memory DbContext for testing using a dedicated EF service provider
                    var efServiceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<MisticVaultDbContext>(options =>
                        options.UseInMemoryDatabase(dbName)
                               .UseInternalServiceProvider(efServiceProvider));
                });
            });

            // Seed DB before creating client so server sees seeded data
            var categoryId = Guid.NewGuid();
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MisticVaultDbContext>();
                await db.Database.EnsureCreatedAsync();

                var category = new MisticVault.Core.Todo.Entities.TodoCategory
                {
                    Id = categoryId,
                    Name = "Cat1",
                    Description = "d",
                    Color = "#FFF"
                };
                db.TodoCategories.Add(category);
                await db.SaveChangesAsync();
            }

            var client = factory.CreateClient();

            var create = new CreateTodoRequestDTO
            {
                Title = "IT-1",
                Description = "desc",
                Priority = MisticVault.Core.Todo.Enums.TodoEnums.TodoPriority.Low,
                CategoryId = categoryId
            };

            var resp = await client.PostAsJsonAsync("/api/todo", create);
            resp.EnsureSuccessStatusCode();

            var created = await resp.Content.ReadFromJsonAsync<TodoResponseDTO>();
            Assert.NotNull(created);
            Assert.Equal("IT-1", created!.Title);

            var get = await client.GetFromJsonAsync<TodoResponseDTO[]>($"/api/todo");
            Assert.NotNull(get);
            Assert.Contains(get, t => t.Id == created.Id);
        }
    }
}
