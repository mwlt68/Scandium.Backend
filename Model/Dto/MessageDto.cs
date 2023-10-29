namespace Scandium.Model.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string? Content { get; set; }
    }
}