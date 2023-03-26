using Scandium.Model.Entities;

namespace Scandium.Data.Abstract
{
    public interface IMessageRepository:IGenericRepository<Message>
    {
        Task<List<Message>> GetAllMessagesAsync(Guid currentUserId);
    }
}