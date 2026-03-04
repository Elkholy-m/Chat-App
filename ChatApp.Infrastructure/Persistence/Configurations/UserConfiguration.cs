using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("USERS_PK");

        builder.HasIndex(e => e.Email, "USERS_EMAIL_UNQ").IsUnique();

        builder.HasIndex(e => e.UserName, "USERS_USERNAME_UNQ").IsUnique();

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.PasswordHash).HasMaxLength(256);
        builder.Property(e => e.UserName).HasMaxLength(128);
    }
}
