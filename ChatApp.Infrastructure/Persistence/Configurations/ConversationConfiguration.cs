using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(e => e.Id).HasName("CONVERSATIONS_PK");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
    }
}
