using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

public interface IMessageRepository
{
    Task CreateMessageAsync(Message message);

    Task<IList<Message>> GetMessagesForConversationAsync(
        Guid conversationId,
        int pageSize,
        int pageNumber);

    Task<Message?> GetByIdAsync(Guid messageId);
}
