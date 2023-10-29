using Scandium.Helpers;
using Scandium.Model.Entities;

namespace Scandium.Model.Dto
{
    public class MessageResponseDto
    {
        public MessageResponseDto(){}
        public static async Task<MessageResponseDto> Get(Message message)
        {
            var decryptedContent = await AES.DecryptAsync(message.Contents!);
            return new MessageResponseDto()
            {
                Id = message.Id,
                Sender = UserResponseDto.Get(message.Sender),
                Receiver = UserResponseDto.Get(message.Receiver),
                Content = decryptedContent,
                CreateDate = message.CreatedAt,
            };
        }



        public Guid Id { get; set; }
        public UserResponseDto? Sender { get; set; }
        public UserResponseDto? Receiver { get; set; }
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}