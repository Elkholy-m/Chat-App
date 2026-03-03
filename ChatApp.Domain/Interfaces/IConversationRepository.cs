using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

public interface IConversationRepository
{
    Task CreateAsync(Conversation conversation);

    Task<Conversation?> GetByIdAsync(Guid conversationId);

    Task<IList<Conversation>> GetUserConversationsAsync(Guid userId);

    Task<bool> ExistsPrivateConversationAsync(Guid userA, Guid userB);
}
