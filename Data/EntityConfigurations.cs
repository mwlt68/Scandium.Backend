
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
            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }
    }
    public class UserConfigurations : BaseEntityConfigurations<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder
                .HasMany(u => u.ReceiverMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId)
                .IsRequired();
            builder
                .HasMany(u => u.SenderMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .IsRequired();
            builder
                .HasMany(u => u.ReceiverFriendshipRequests)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId)
                .IsRequired();
            builder
                .HasMany(u => u.SenderFriendshipRequests)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .IsRequired();
        }
    }
    public class MessageConfigurations : BaseEntityConfigurations<Message>
    {
        public override void Configure(EntityTypeBuilder<Message> builder)
        {
            base.Configure(builder);
        }
    }

    public class FriendshipRequestConfigurations : BaseEntityConfigurations<FriendshipRequest>
    {
        public override void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            base.Configure(builder);
        }
    }
}