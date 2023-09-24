namespace Scandium.Model.Dto
{
    public class UserSearchResponseDto
    {
        public UserResponseDto UserResponseDto { get; set; }

        public UserSearchResponseDto(UserResponseDto userResponseDto,FriendshipRequestStatus friendshipRequestStatus,Guid? friendshipRequestId=null,Guid? receiverId=null)
        {
            UserResponseDto = userResponseDto;
            FriendshipRequestStatus =friendshipRequestStatus;
            ReceiverId = receiverId;
            FriendshipRequestId = friendshipRequestId;
        }

        public Guid? FriendshipRequestId { get; set; }
        public Guid? ReceiverId { get; set; }

        public FriendshipRequestStatus FriendshipRequestStatus { get; set; }
    }


    public enum FriendshipRequestStatus {
        Following,
        Follow,
        Approve,
        Requested
    }
}