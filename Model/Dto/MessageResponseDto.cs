using Scandium.Model.Entities;

namespace Scandium.Model.Dto
{
    public class MessageResponseDto
    {
        public MessageResponseDto(Message message)
        {
            Id = message.Id;
            Sender = message.Sender != null ? new UserResponseDto(message.Sender): null;
            Receiver = message.Receiver != null ? new UserResponseDto(message.Receiver): null;
            Content = message.Content;
            CreateDate = message.CreatedAt;
        }

        public Guid Id { get; set; }
        public UserResponseDto? Sender { get; set; }
        public UserResponseDto? Receiver { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}