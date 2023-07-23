using Scandium.Model.Entities;

namespace Scandium.Data.Abstract
{
    public interface IFriendshipRequestRepository: IGenericRepository<FriendshipRequest>
    {
        Task<List<FriendshipRequest?>> GetAllAcceptedAsync(Guid currentUserId);
    }
}