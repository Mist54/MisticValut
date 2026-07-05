using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MisticVault.Core.Todo.DTOs.Todo;

namespace MisticVault.Core.Todo.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoResponseDTO>> GetAllAsync();
        Task<TodoResponseDTO?> GetByIdAsync(Guid id);
        Task<TodoResponseDTO> CreateAsync(CreateTodoRequestDTO request);
        Task<TodoResponseDTO?> UpdateAsync(Guid id, UpdateTodoRequestDTO request);
        Task<bool> DeleteAsync(Guid id);
    }
}
