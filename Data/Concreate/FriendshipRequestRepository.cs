using Scandium.Data;
using Scandium.Data.Abstract;
using Scandium.Data.Concreate;
using Scandium.Model.Entities;

public class FriendshipRequestRepository : GenericRepository<FriendshipRequest>, IFriendshipRequestRepository
{
    public FriendshipRequestRepository(AppDbContext context) : base(context)
    {
    }
}