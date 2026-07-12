using MisticVault.Application.Todo.DTOs.Todo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MisticVault.Application.Todo.Interfaces
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
