using Microsoft.AspNetCore.Authorization;
using Scandium.Model.Dto;

namespace Scandium.Hubs
{
    [Authorize]
    public class FriendshipRequestHub : BaseHub<IFriendshipRequesClient>
    {

    }
    public interface IFriendshipRequesClient
    {
        Task GetFriendshipRequest(FriendshipResponseDto dto);
        Task ApproveFriendshipRequest(FriendshipResponseDto dto);
    }
}