namespace Scandium.Model.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } =DateTime.UtcNow;
    }
}