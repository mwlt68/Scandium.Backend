using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Scandium.Data.Abstract;
using Scandium.Model;

namespace Scandium.Data.Concreate
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public readonly AppDbContext context;
        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }

        public virtual DbSet<TEntity> GetDbSet => context.Set<TEntity>();
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null
                    ? await GetDbSet.ToListAsync()
                    : await GetDbSet.Where(filter).ToListAsync();
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetDbSet.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await GetDbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await GetDbSet.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            GetDbSet.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var deleteEntity = await GetByIdThrowAsync(id);
            GetDbSet.Remove(deleteEntity);
            var data = await context.SaveChangesAsync();
            if (data > 0)
                return true;
            else return false;
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await GetDbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetByIdThrowAsync(Guid id)
        {
            return await GetDbSet.FindAsync(id) ?? throw new KeyNotFoundException("Entity not found !");
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetDbSet.AnyAsync(filter);
        }

    }
}