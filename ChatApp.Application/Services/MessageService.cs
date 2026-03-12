using ChatApp.Application.Dtos.Messages;
using ChatApp.Application.Exceptions;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services;

public class MessageService(IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IUnitOfWork unitOfWork) : IMessageService
{
    public async Task<IList<MessageDto>> GetMessageForConversation(Guid conversationId, Guid userId, int pageSize, int pageNumber)
    {
        await CheckConversationExistance(conversationId, userId);

        IList<Message> messages = await messageRepository.GetMessagesForConversationAsync(conversationId, pageSize, pageNumber);

        IList<MessageDto> messageDtos = [];
        foreach (Message message in messages) {
            messageDtos.Add(MapIntoMessageDto(message));
        }

        return messageDtos;
    }

    public async Task<MessageDto> GetMessageById(Guid conversationId, Guid userId, Guid messageId)
    {
        await CheckConversationExistance(conversationId, userId);

        Message message = await messageRepository.GetByIdAsync(messageId) ??
            throw new NotFoundException("Message with id {messageId} not in the db.");

        return MapIntoMessageDto(message);
    }

    public async Task CreateMessage(Guid conversationId, Guid senderId, SendMessageDto sendMessage)
    {
        await CheckConversationExistance(conversationId, senderId);

        ValidateMessage(sendMessage.Content);

        Message dbMsg = new (Guid.NewGuid(),
                conversationId,
                senderId,
                sendMessage.Content);

        await messageRepository.CreateMessageAsync(dbMsg);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeMessageContent(Guid conversationId, Guid senderId, Guid messageId, string newContent)
    {
        await CheckConversationExistance(conversationId, senderId);

        Message message = await messageRepository.GetByIdAsync(messageId) ??
            throw new NotFoundException("Message with id {messageId} not in the db.");

        ValidateMessage(newContent);

        message.Delete();

        Message dbMsg = new (Guid.NewGuid(),
                conversationId,
                senderId,
                newContent);

        await messageRepository.CreateMessageAsync(dbMsg);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMessage(Guid conversationId, Guid senderId, Guid messageId)
    {
        await CheckConversationExistance(conversationId, senderId);

        Message message = await messageRepository.GetByIdAsync(messageId) ??
            throw new NotFoundException("Message with id {messageId} not in the db.");

        if (message.SenderId != senderId)
            throw new UnauthorizedException("You can't delete another user message.");

        message.Delete();
        await unitOfWork.SaveChangesAsync();
    }

    private async Task CheckConversationExistance(Guid conversationId, Guid userId) {
        Conversation conversation = await conversationRepository.GetByIdAsync(conversationId) ??
            throw new NotFoundException($"Conversation with id : {conversationId} not exist in db.");

        if (!conversation.HasParticipant(userId))
            throw new UnauthorizedException($"User with id : {userId} not exist in conversation.");
    }

    private static MessageDto MapIntoMessageDto(Message message) {
        MessageDto messageDto = new () {
            Id = message.Id,
            Content = message.Content,
            IsRead = message.IsRead,
            SentAt = message.SentAt
        };

        return messageDto;
    }

    private static void ValidateMessage(string message) {
        if (string.IsNullOrWhiteSpace(message))
            throw new BadRequestException("Message content cannot be empty.");

        if (message.Length > 3000)
            throw new BadRequestException("Message content is to big slice it to sub messages.");
    }

}
