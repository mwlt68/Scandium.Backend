using Scandium.Model.Dto;

namespace Scandium.Features.Message.Conversation
{
    public class Request
    {
        public Guid OtherUserId { get; set; }
    }

    public class ConversationResponseModel
    {
        public List<Scandium.Model.Dto.MessageDto> Messages { get; set; }

        public ConversationResponseModel(Model.Entities.User? CurrentUser, Model.Entities.User? OtherUser, List<MessageDto> Messages)
        {
            this.Messages = Messages;
            this.CurrentUser = UserResponseDto.Get(CurrentUser);
            this.OtherUser = UserResponseDto.Get(OtherUser);
        }

        public UserResponseDto? CurrentUser { get; set; }
        public UserResponseDto? OtherUser { get; set; }
    }

}