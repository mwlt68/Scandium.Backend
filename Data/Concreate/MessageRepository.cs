
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

        public async Task<List<Message>> GetConversationAsync(Guid currentUserId, Guid oppositeUserId)
        {
            return await GetDbSet
                .OrderBy(x => x.CreatedAt)
                .Where(x => (x.SenderId == currentUserId  && x.ReceiverId == oppositeUserId) || (x.ReceiverId == currentUserId && x.SenderId == oppositeUserId))
                .ToListAsync();
        }

        public override IQueryable<Message> GetDefaultQueyable()
        {
            return base.GetDefaultQueyable()
                .Include(x => x.Sender)
                .Include(x => x.Receiver);
        }
        public virtual async Task<List<Message?>> GetLastMessagesAsync(Guid currentUserId)
        {
            return await GetDefaultQueyable()
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.SenderId == currentUserId || x.ReceiverId == currentUserId)
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault()).ToListAsync();
        }
    }
}