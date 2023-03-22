using System.ComponentModel.DataAnnotations;

namespace Scandium.Model
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt
        {
           get
           {
              return this.dateCreated.HasValue
                 ? this.dateCreated.Value
                 : DateTime.UtcNow;
           }

           set { this.dateCreated = value; }
        }

        private DateTime? dateCreated = null;
    }
}