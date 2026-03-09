using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("USERS_PK");

        builder.HasIndex(u => u.Email, "IX_Users_Email_NotDeleted")
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(u => u.UserName, "IX_Users_Username_NotDeleted")
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        builder.Property(e => e.Email).HasMaxLength(256);
        builder.Property(e => e.PasswordHash).HasMaxLength(256);
        builder.Property(e => e.UserName).HasMaxLength(128);
    }
}
