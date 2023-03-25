
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Scandium.Model;

namespace Scandium.Extensions.QueryableExtensions
{
    public static class GenericRepositoryQueryableExtension
    {
        public static async Task<TEntity?> GetAsync<TEntity>(this IQueryable<TEntity> queryable,Expression<Func<TEntity, bool>> filter) where TEntity : BaseEntity, new()
        {
            return await queryable.FirstOrDefaultAsync(filter);
        }
        public static async Task<List<TEntity>> GetListAsync<TEntity>(this IQueryable<TEntity> queryable,Expression<Func<TEntity, bool>>? filter = null) where TEntity : BaseEntity, new()
        {
            return filter == null
                    ? await queryable.ToListAsync()
                    : await queryable.Where(filter).ToListAsync();
        }
        public static async Task<TEntity?> GetById<TEntity>(this IQueryable<TEntity> queryable,Guid id) where TEntity : BaseEntity, new()
        {
            return await queryable.FirstOrDefaultAsync(x=> x.Id == id);
        }
    }
}