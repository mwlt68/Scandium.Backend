using Scandium.Exceptions;
using Scandium.Model;

namespace Scandium.Extensions.EntityExtensions
{
    public static class EntityExtension
    {
        public static async Task<TEntity> ThrowIfNotFound<TEntity>(this Task<TEntity?> entityTask) where TEntity : BaseEntity, new()
        {
            var entity =await entityTask;
            if ( entity == null)
                throw new NotFoundException(typeof(TEntity));
            else return entity;
        }
    }
}