using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MisticVault.Application.Todo.Interfaces;
using MisticVault.Core.Todo.Entities;
using MisticVault.Infrastructure.Data;

namespace MisticVault.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly MisticVaultDbContext _db;

        public TodoRepository(MisticVaultDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(MisticVault.Core.Todo.Entities.Todo entity)
        {
            await _db.Set<MisticVault.Core.Todo.Entities.Todo>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(MisticVault.Core.Todo.Entities.Todo entity)
        {
            _db.Set<MisticVault.Core.Todo.Entities.Todo>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<MisticVault.Core.Todo.Entities.Todo>> GetAllAsync()
        {
            return await _db.Set<MisticVault.Core.Todo.Entities.Todo>().Include(t => t.Category).ToListAsync();
        }

        public async Task<MisticVault.Core.Todo.Entities.Todo?> GetByIdAsync(Guid id)
        {
            return await _db.Set<MisticVault.Core.Todo.Entities.Todo>().Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(MisticVault.Core.Todo.Entities.Todo entity)
        {
            _db.Set<MisticVault.Core.Todo.Entities.Todo>().Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}

