
using Microsoft.EntityFrameworkCore;
using Scandium.Data.Abstract;
using Scandium.Model.Entities;

namespace Scandium.Data.Concreate
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }
        public virtual async Task<Message> GetByIdThrowAsync(Guid id)
        {
            return await GetDbSet
                .Include(x=> x.Sender)
                .Include(x=> x.Receiver)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Entity not found !");
        }
    }
}