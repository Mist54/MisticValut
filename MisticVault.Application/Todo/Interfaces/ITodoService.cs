using MisticVault.Core.Todo.DTOs.Todo;

namespace MisticVault.Core.Todo.Interfaces
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
