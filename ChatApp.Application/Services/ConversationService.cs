using ChatApp.Application.Dtos.Conversation;
using ChatApp.Application.Exceptions;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services;

public class ConversationService(
        IUserRepository userRepository,
        IConversationRepository conversationRepository,
        IUnitOfWork unitOfWork) : IConversationService
{
    public async Task<Guid> CreateConversationAsync(IList<Guid> participantIds)
    {
        if (participantIds.Count < 2)
            throw new BadRequestException("Can't create a conversation for less than 2 users.");

        // CHECK IF CONVERSATION EXISTANCE FIRST BEFORE CREATE PRIVATE CONVERSATION
        if (participantIds.Count == 2) {
            IList<Conversation> intersectedConversations = await conversationRepository
                .IntersectedConversations(participantIds[0], participantIds[1]);

            Conversation? privateConversation = intersectedConversations
                .FirstOrDefault(c => c.ConversationParticipants.Count == 2);

            if (privateConversation != null) {
                return privateConversation.Id;
            }
        }


        var conversation = new Conversation(Guid.NewGuid());

        foreach (Guid pId in participantIds) {
            if (!await userRepository.CheckUserExistance(pId))
                throw new BadRequestException($"User with id : {pId} not exist in system.");

            conversation.AddParticipant(pId);
        }

        await conversationRepository.CreateAsync(conversation);
        await unitOfWork.SaveChangesAsync();

        return conversation.Id;
    }

    public async Task DeleteConversation(Guid convId)
    {
        Conversation conversation = await conversationRepository.GetByIdAsync(convId) ??
            throw new NotFoundException($"Conversation with id : {convId} not exist in db.");

        conversation.DeleteConversation();
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ConversationResponse> GetConversationByIdAsync(Guid convId)
    {
        Conversation conv = await conversationRepository.GetByIdAsync(convId) ??
            throw new NotFoundException($"Conversation with id : {convId} not exist in db.");

        return new ConversationResponse
        {
            Id = conv.Id,
            CreatedAt = conv.CreatedAt,
            ParticipantsIds = [.. conv.ConversationParticipants.Select(cp => cp.UserId)]
        };
    }

    public async Task<IList<ConversationResponse>> GetConversationsForUserAsync(Guid userId)
    {
        IList<Conversation> conversations = await conversationRepository.GetUserConversationsAsync(userId);

        return [.. conversations.Select(c => new ConversationResponse {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                ParticipantsIds = [.. c.ConversationParticipants.Select(cp => cp.UserId)]
                })];
    }

    public async Task AddUsersToConversation(Guid convId, IList<Guid> userIds)
    {
        Conversation conversation = await conversationRepository.GetByIdIgnoreFilterAsync(convId) ??
            throw new NotFoundException($"Conversation with id : {convId} not exists in db");

        foreach (var userId in userIds) {
            if (!conversation.HasParticipant(userId)) {
                conversation.AddParticipant(userId);
            } else {
                conversation.RestoreParticipant(userId);
            }
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUsersFromConversation(Guid convId, Guid userId)
    {
        Conversation conversation = await conversationRepository.GetByIdAsync(convId) ??
            throw new NotFoundException($"Conversation with id : {convId} not exists in db");

        conversation.ConversationParticipants
            .FirstOrDefault(cp => cp.UserId == userId)?
            .DeleteParticipant();

        if (conversation.ConversationParticipants.Count < 3)
            conversation.DeleteConversation();

        await unitOfWork.SaveChangesAsync();
    }
}

