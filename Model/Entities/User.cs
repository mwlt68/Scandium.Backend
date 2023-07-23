using System.ComponentModel.DataAnnotations;

namespace Scandium.Model.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(25)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public virtual ICollection<Message>? ReceiverMessages { get; set; }
        public virtual ICollection<Message>? SenderMessages { get; set; }
        public virtual ICollection<FriendshipRequest>? ReceiverFriendshipRequests { get; set; }
        public virtual ICollection<FriendshipRequest>? SenderFriendshipRequests { get; set; }
    }
}