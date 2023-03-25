
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scandium.Model;
using Scandium.Model.Entities;

namespace Scandium.Data
{
    public class BaseEntityConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("timezone('utc', now())");
        }
    }
    public class UserConfigurations : BaseEntityConfigurations<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
        }
    }
    public class MessageConfigurations : BaseEntityConfigurations<Message>
    {
        public override void Configure(EntityTypeBuilder<Message> builder)
        {
            base.Configure(builder);
        }
    }
}