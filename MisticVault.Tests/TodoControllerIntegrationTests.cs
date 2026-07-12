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

            // create category first
            var category = new { Id = Guid.NewGuid(), Name = "Cat1", Description = "d", Color = "#FFF" };
            // insert category directly via scope
            using (var scope = clientFactory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MisticVaultDbContext>();
                db.Set<MisticVault.Core.Todo.Entities.TodoCategory>().Add(new MisticVault.Core.Todo.Entities.TodoCategory
                {
                    Id = (Guid)category.Id,
                    Name = "Cat1",
                    Description = "d",
                    Color = "#FFF"
                });
                await db.SaveChangesAsync();
            }

            var create = new CreateTodoRequestDTO { Title = "IT-1", Description = "desc", Priority = MisticVault.Core.Todo.Enums.TodoEnums.TodoPriority.Low, CategoryId = (Guid)category.Id };
            var resp = await client.PostAsJsonAsync("/api/todo", create);
            resp.EnsureSuccessStatusCode();

            var created = await resp.Content.ReadFromJsonAsync<TodoResponseDTO>();
            Assert.NotNull(created);
            Assert.Equal("IT-1", created!.Title);

            var get = await client.GetFromJsonAsync<TodoResponseDTO[]>($"/api/todo");
            Assert.Contains(get!, t => t.Id == created.Id);
        }
    }
}
