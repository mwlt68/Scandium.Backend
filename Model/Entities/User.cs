using System.ComponentModel.DataAnnotations;

namespace Scandium.Model
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(25)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(25)]
        public string? Password { get; set; }
    }
}