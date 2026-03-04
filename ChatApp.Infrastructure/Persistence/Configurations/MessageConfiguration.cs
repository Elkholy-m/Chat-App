using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(e => e.Id).HasName("MESSAGES_PK");

        builder.HasIndex(e => e.ConversationId, "MESSAGES_CONVERSATIONID_IDX");

        builder.HasIndex(e => new { e.ConversationId, e.SentAt }, "MESSAGES_CONVERSATIONID_SENTAT_DESC_IDX").IsDescending(false, true);

        builder.HasIndex(e => e.SenderId, "MESSAGES_SENDERID_IDX");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.SentAt).HasDefaultValueSql("(sysutcdatetime())");

        builder.HasOne(d => d.Conversation).WithMany(p => p.Messages)
            .HasForeignKey(d => d.ConversationId)
            .HasConstraintName("MESSAGES_CONVERSATIONID_FK");

        builder.HasOne(d => d.Sender).WithMany(p => p.Messages)
            .HasForeignKey(d => d.SenderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("MESSAGES_USERID_FK");
    }
}
