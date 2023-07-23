
using System.ComponentModel.DataAnnotations;

namespace Scandium.Model.Entities
{
    public class FriendshipRequest: BaseEntity
    {
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid ReceiverId { get; set; }
        public bool IsApproved { get; set; } = false;
        public virtual User? Receiver { get; set; }
        public virtual User? Sender { get; set; }

    }
}