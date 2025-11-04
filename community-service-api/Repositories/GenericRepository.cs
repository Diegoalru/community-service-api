using System.Collections.Generic;
using System.Threading.Tasks;
using community_service_api.DbContext;
using Microsoft.EntityFrameworkCore;

namespace community_service_api.Repositories;

public class GenericRepository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(params object[] keyValues)
    {
        return await _dbSet.FindAsync(keyValues);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(params object[] keyValues)
    {
        var entity = await GetByIdAsync(keyValues);
        if (entity is null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
