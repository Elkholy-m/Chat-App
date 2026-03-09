using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> context) : base(context) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ConversationParticipantConfiguration());

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Conversation>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<ConversationParticipant>().HasQueryFilter(cp => !cp.IsDeleted);
        modelBuilder.Entity<Message>().HasQueryFilter(m => !m.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationParticipant> ConversationParticipants { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }
}
