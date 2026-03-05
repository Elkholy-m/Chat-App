using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

public interface IConversationParticipantRepository
{
    Task CreateParticipantAsync(ConversationParticipant participant);

    Task<IList<Guid>> GetConversationIdsForUser(Guid userId);

    Task<IList<Guid>> GetUserIdsForConversation(Guid conversationId);

    Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId);

    Task<ConversationParticipant?> GetParticipantAsync(Guid conversationId, Guid userId);

    void RemoveParticipant(ConversationParticipant participant);
}
