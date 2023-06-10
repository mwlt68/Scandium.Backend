
using System.ComponentModel.DataAnnotations;

namespace Scandium.Model.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid ReceiverId { get; set; }
        [Required]
        public string? Content { get; set; }
        public virtual User? Receiver { get; set; }
        public virtual User? Sender { get; set; }

    }
}