using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MisticVault.Infrastructure.Data;
using MisticVault.Infrastructure.Repositories;
using MisticVault.Infrastructure.Services;
using MisticVault.Application.Todo.DTOs.Todo;
using Xunit;

namespace MisticVault.Tests
{
    public class TodoServiceTests
    {
        private static MisticVaultDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MisticVaultDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new MisticVaultDbContext(options);
        }

        [Fact]
        public async Task Create_Get_Update_Delete_Workflow()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateInMemoryContext(dbName);

            // ensure category exists
            var category = new MisticVault.Core.Todo.Entities.TodoCategory { Id = Guid.NewGuid(), Name = "Test", Color = "#000" };
            ctx.Set<MisticVault.Core.Todo.Entities.TodoCategory>().Add(category);
            await ctx.SaveChangesAsync();

            var repo = new TodoRepository(ctx);
            // create mapper using application's profile
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile(new MisticVault.Application.Todo.Mappings.TodoProfile()));
            var mapper = mapperConfig.CreateMapper();
            var service = new TodoService(repo, mapper);

            var create = new CreateTodoRequestDTO { Title = "T1", Description = "D1", Priority = MisticVault.Core.Todo.Enums.TodoEnums.TodoPriority.Medium, CategoryId = category.Id };
            var created = await service.CreateAsync(create);

            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.Equal("T1", created.Title);

            var all = (await service.GetAllAsync()).ToList();
            Assert.Single(all);

            var fetched = await service.GetByIdAsync(created.Id);
            Assert.NotNull(fetched);

            var updateReq = new UpdateTodoRequestDTO { Title = "T1-up", Description = "D1-up", Priority = MisticVault.Core.Todo.Enums.TodoEnums.TodoPriority.High, Status = MisticVault.Core.Todo.Enums.TodoEnums.TodoStatus.InProgress, CategoryId = category.Id };
            var updated = await service.UpdateAsync(created.Id, updateReq);
            Assert.NotNull(updated);
            Assert.Equal("T1-up", updated!.Title);

            var deleted = await service.DeleteAsync(created.Id);
            Assert.True(deleted);

            var after = (await service.GetAllAsync()).ToList();
            Assert.Empty(after);
        }
    }
}
