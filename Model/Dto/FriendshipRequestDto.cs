
using Scandium.Model.Entities;

namespace Scandium.Model.Dto
{
    public class FriendshipRequestDto
    {
        public FriendshipRequestDto(FriendshipRequest friendshipRequest)
        {
            Id = friendshipRequest.Id;
            Sender = UserResponseDto.Get(friendshipRequest.Sender);
            Receiver =UserResponseDto.Get(friendshipRequest.Receiver);
            IsApproved = friendshipRequest.IsApproved;
            CreateDate = friendshipRequest.CreatedAt;
        }
        public Guid Id { get; set; }
        public UserResponseDto? Sender { get; set; }
        public UserResponseDto? Receiver { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
    }
}