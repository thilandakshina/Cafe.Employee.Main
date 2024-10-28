using Cafe.Data.Context;
using Cafe.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CafeDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(CafeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
