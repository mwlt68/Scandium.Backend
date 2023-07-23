using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using Scandium.Data.Abstract;
using Scandium.Data.Concreate;
using Scandium.Model.Entities;

public class FriendshipRequestRepository : GenericRepository<FriendshipRequest>, IFriendshipRequestRepository
{
    public FriendshipRequestRepository(AppDbContext context) : base(context)
    {
    }
    public override IQueryable<FriendshipRequest> GetDefaultQueyable()
    {
        return base.GetDefaultQueyable()
            .Include(x => x.Sender)
            .Include(x => x.Receiver);
    }
    public async Task<List<FriendshipRequest?>> GetAllAcceptedAsync(Guid currentUserId)
    {
        return await GetDefaultQueyable()
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.SenderId == currentUserId || x.ReceiverId == currentUserId && x.IsApproved)
            .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Select(g => g.FirstOrDefault()).ToListAsync();
    }
}