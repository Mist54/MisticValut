using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MisticVault.Application.Todo.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<MisticVault.Core.Todo.Entities.Todo>> GetAllAsync();
        Task<MisticVault.Core.Todo.Entities.Todo?> GetByIdAsync(Guid id);
        Task AddAsync(MisticVault.Core.Todo.Entities.Todo entity);
        Task UpdateAsync(MisticVault.Core.Todo.Entities.Todo entity);
        Task DeleteAsync(MisticVault.Core.Todo.Entities.Todo entity);
    }
}
