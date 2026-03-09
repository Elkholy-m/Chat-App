using ChatApp.Application.Dtos.Conversation;

namespace ChatApp.Application.Interfaces.Services;

public interface IConversationService
{
    Task<Guid> CreateConversationAsync(IList<Guid> participantIds);

    Task<ConversationResponse> GetConversationByIdAsync(Guid convId);

    Task<IList<ConversationResponse>> GetConversationsForUserAsync(Guid userId);

    Task DeleteConversation(Guid convId);

    Task AddUsersToConversation(Guid convId, IList<Guid> userIds);

    Task DeleteUsersFromConversation(Guid convId, Guid userId);
}
