using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MisticVault.Core.Todo.DTOs.Todo;
using MisticVault.Core.Todo.Services;
using MisticVault.Infrastructure.Data;
using MisticVault.Core.Todo.Entities;

namespace MisticVault.Infrastructure.Todo.Services
{
    public class TodoService : ITodoService
    {
        private readonly MisticVaultDbContext _db;

        public TodoService(MisticVaultDbContext db)
        {
            _db = db;
        }

        public async Task<TodoResponseDTO> CreateAsync(CreateTodoRequestDTO request)
        {
            var entity = new MisticVault.Core.Todo.Entities.Todo
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                Priority = request.Priority,
                CategoryId = request.CategoryId,
                Status = MisticVault.Core.Todo.Enums.TodoEnums.TodoStatus.NotStarted
            };

            _db.Set<MisticVault.Core.Todo.Entities.Todo>().Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Set<MisticVault.Core.Todo.Entities.Todo>().FindAsync(id);
            if (entity == null) return false;

            _db.Set<MisticVault.Core.Todo.Entities.Todo>().Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TodoResponseDTO>> GetAllAsync()
        {
            var items = await _db.Set<MisticVault.Core.Todo.Entities.Todo>().Include(t => t.Category).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<TodoResponseDTO?> GetByIdAsync(Guid id)
        {
            var item = await _db.Set<MisticVault.Core.Todo.Entities.Todo>().Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<TodoResponseDTO?> UpdateAsync(Guid id, UpdateTodoRequestDTO request)
        {
            var entity = await _db.Set<MisticVault.Core.Todo.Entities.Todo>().FindAsync(id);
            if (entity == null) return null;

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.DueDate = request.DueDate;
            entity.Priority = request.Priority;
            entity.Status = request.Status;
            entity.CategoryId = request.CategoryId;
            entity.UpdatedDate = DateTime.UtcNow;

            _db.Set<MisticVault.Core.Todo.Entities.Todo>().Update(entity);
            await _db.SaveChangesAsync();

            // reload category
            await _db.Entry(entity).Reference(e => e.Category).LoadAsync();

            return MapToDto(entity);
        }

        private static TodoResponseDTO MapToDto(MisticVault.Core.Todo.Entities.Todo entity)
        {
            return new TodoResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Priority = entity.Priority,
                Status = entity.Status,
                Category = entity.Category == null ? null : new MisticVault.Core.Todo.DTOs.Todo.TodoCategoryResponseDTO
                {
                    Id = entity.Category.Id,
                    Name = entity.Category.Name,
                    Color = entity.Category.Color
                }
            };
        }
    }
}
