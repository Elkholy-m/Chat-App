using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class ConversationParticipantConfiguration 
    : IEntityTypeConfiguration<ConversationParticipant>
{
    public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        builder.HasKey(e => new { e.ConversationId, e.UserId }).HasName("CONVPART_PK");

        builder.HasIndex(e => e.ConversationId, "CONVPART_CONVERSATIONID_IDX");

        builder.HasIndex(e => e.UserId, "CONVPART_USERID_IDX");

        builder.Property(e => e.JoinedAt).HasDefaultValueSql("(sysutcdatetime())");

        builder.HasOne(d => d.Conversation).WithMany(p => p.ConversationParticipants)
            .HasForeignKey(d => d.ConversationId)
            .HasConstraintName("CONVPART_CONVERSATIONID_FK");

        builder.HasOne(d => d.User).WithMany(p => p.ConversationParticipants)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("CONVPART_USERID_FK");
    }
}
