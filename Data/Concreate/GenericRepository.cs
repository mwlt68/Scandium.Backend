using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scandium.Data.Abstract;
using Scandium.Model;

namespace Scandium.Data.Concreate
{
    public class GenericRepository <TEntity> :IGenericRepository <TEntity> where TEntity : BaseEntity, new()
    {
        public readonly AppDbContext context;
        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null
                    ? await context.Set<TEntity>().ToListAsync()
                    : await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            TEntity? deleteEntity = await context.Set<TEntity>().FindAsync(id);
            if (deleteEntity != null)
            {
                context.Set<TEntity>().Remove(deleteEntity);
                var data = await context.SaveChangesAsync();
                if (data > 0)
                    return true;
                else return false;
            }
            throw new KeyNotFoundException("Entity not found !");
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().AnyAsync(filter);
        }
    }
}