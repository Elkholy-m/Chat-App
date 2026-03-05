using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

public class ConversationParticipantRepository(AppDbContext context) : IConversationParticipantRepository
{
    private readonly DbSet<ConversationParticipant> _participants = context.Set<ConversationParticipant>();

    public async Task CreateParticipantAsync(ConversationParticipant participant) =>
        await _participants.AddAsync(participant);
        
    public async Task<IList<Guid>> GetConversationIdsForUser(Guid userId) =>
        await _participants
            .AsNoTracking()
            .Where(cp => cp.UserId == userId)
            .Select(cp => cp.ConversationId)
            .ToListAsync();

    public async Task<IList<Guid>> GetUserIdsForConversation(Guid conversationId) => 
        await _participants
            .AsNoTracking()
            .Where(cp => cp.ConversationId == conversationId)
            .Select(cp => cp.UserId)
            .ToListAsync();

    public async Task<ConversationParticipant?> GetParticipantAsync(Guid conversationId, Guid userId) =>
        await _participants
            .FirstOrDefaultAsync(cp => cp.UserId == userId &&
                    cp.ConversationId == conversationId);

    public async Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId) =>
        await _participants
            .AsNoTracking()
            .AnyAsync(cp => cp.UserId == userId &&
                    cp.ConversationId == conversationId);

    public void RemoveParticipant(ConversationParticipant participant) =>
        _participants.Remove(participant);
}
