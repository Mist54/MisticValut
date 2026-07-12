using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var clientFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MisticVaultDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<MisticVaultDbContext>(options =>
                        options.UseInMemoryDatabase("IntegrationTestDb"));
                });
            });

            var client = clientFactory.CreateClient();

            // Create category first
            var categoryId = Guid.NewGuid();
            using (var scope = clientFactory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MisticVaultDbContext>();

                // Ensure the database is created
                await db.Database.EnsureCreatedAsync();

                var category = new MisticVault.Core.Todo.Entities.TodoCategory
                {
                    Id = categoryId,
                    Name = "Cat1",
                    Description = "d",
                    Color = "#FFF"
                };
                db.Add(category);  // Use db.Add() instead of db.Set<>().Add()
                await db.SaveChangesAsync();
            }

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
            Assert.NotNull(get);  // Add null check
            Assert.Contains(get, t => t.Id == created.Id);
        }
    }
}
