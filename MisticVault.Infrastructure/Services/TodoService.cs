using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MisticVault.Application.Todo.Interfaces;
using MisticVault.Application.Todo.DTOs.Todo;
using MisticVault.Core.Todo.Entities;

namespace MisticVault.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repo;
        private readonly AutoMapper.IMapper _mapper;

        public TodoService(ITodoRepository repo, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TodoResponseDTO> CreateAsync(CreateTodoRequestDTO request)
        {
            var entity = _mapper.Map<MisticVault.Core.Todo.Entities.Todo>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Status = MisticVault.Core.Todo.Enums.TodoEnums.TodoStatus.NotStarted;

            await _repo.AddAsync(entity);

            return _mapper.Map<TodoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            return true;
        }

        public async Task<IEnumerable<TodoResponseDTO>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(i => _mapper.Map<TodoResponseDTO>(i));
        }

        public async Task<TodoResponseDTO?> GetByIdAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<TodoResponseDTO>(item);
        }

        public async Task<TodoResponseDTO?> UpdateAsync(Guid id, UpdateTodoRequestDTO request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.DueDate = request.DueDate;
            entity.Priority = request.Priority;
            entity.Status = request.Status;
            entity.CategoryId = request.CategoryId;
            entity.UpdatedDate = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);

            return _mapper.Map<TodoResponseDTO>(entity);
        }
    }
}
