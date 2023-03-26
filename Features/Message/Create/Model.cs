
namespace Scandium.Features.Message.Create
{
    public class Request
    {
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
    }
}