using System.ComponentModel.DataAnnotations;

namespace Scandium.Model
{
    public class User : BaseEntity
    {
        [Required]
        [MinLength(7)]
        public string? Username { get; set; }

        [Required]
        [MinLength(7)]
        public string? Password { get; set; }
    }
}