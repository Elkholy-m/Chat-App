using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

public class ConversationRepository(AppDbContext context) : IConversationRepository
{
    private readonly DbSet<Conversation> _conversations = context.Set<Conversation>();

    public async Task CreateAsync(Conversation conversation) =>
        await _conversations.AddAsync(conversation);

    public async Task<Conversation?> GetByIdAsync(Guid conversationId) =>
        await _conversations
            .Include(c => c.ConversationParticipants)
            .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == conversationId);

    public async Task<IList<Conversation>> IntersectedConversations(Guid userA, Guid userB) =>
        await _conversations
            .Include(c => c.ConversationParticipants)
            .Where(c =>
                    !c.IsDeleted && 
                    c.ConversationParticipants.Any(p => p.UserId == userA) &&
                    c.ConversationParticipants.Any(p => p.UserId == userB))
            .ToListAsync();

    public async Task<IList<Conversation>> GetUserConversationsAsync(Guid userId) =>
        await _conversations
            .Include(c => c.ConversationParticipants)
            .Where(c => !c.IsDeleted && c.ConversationParticipants.Any(cp => cp.UserId == userId))
            .ToListAsync();
}
