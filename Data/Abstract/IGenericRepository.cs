using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Scandium.Model;

namespace Scandium.Data.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        DbSet<TEntity> GetDbSet{get;}
        IQueryable<TEntity> GetDefaultQueyable();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(Guid id);
    }
}