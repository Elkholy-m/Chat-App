using ChatApp.Application.Dtos.Messages;

namespace ChatApp.Application.Interfaces.Services;

public interface IMessageService
{
   Task<MessageDto> GetMessageById(Guid conversationId, Guid userId, Guid messageId);

   Task<IList<MessageDto>> GetMessageForConversation(Guid conversationId, Guid userId, int pageSize, int pageNumber);

   Task CreateMessage(Guid conversationId, Guid senderId, SendMessageDto sendMessage);

   Task ChangeMessageContent(Guid conversationId, Guid senderId, Guid messageId, string newContent);

   Task DeleteMessage(Guid conversationId, Guid senderId, Guid messageId);
}
