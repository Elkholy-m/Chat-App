using ChatApp.Application.Dtos.Conversation;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services;

public class ConversationService(
        IConversationRepository conversationRepository,
        IUnitOfWork unitOfWork) : IConversationService
{
    public async Task<Guid> CreateConversationAsync(IList<Guid> participantIds)
    {
        if (participantIds.Count < 2)
            throw new Exception("Can't create a conversation for less than 2 users.");

        // CHECK IF CONVERSATION EXISTANCE FIRST BEFORE CREATE PRIVATE CONVERSATION
        if (participantIds.Count == 2) {
            IList<Conversation> intersectedConversations = await conversationRepository
                .IntersectedConversations(participantIds[0], participantIds[1]);

            if (intersectedConversations != null)
                Console.WriteLine("INTERSECTED CONVERSATIONS IS NOT NULL");

            Conversation? privateConversation = intersectedConversations!
                .FirstOrDefault(c => c.ConversationParticipants.Count == 2);

            if (privateConversation != null) {
                Console.WriteLine("PRIVATE CONVERSATION IS NOT NULL");
                return privateConversation.Id;
            }
        }


        var conversation = new Conversation(Guid.NewGuid());

        foreach (Guid pId in participantIds) {
            conversation.AddParticipant(pId);
        }

        await conversationRepository.CreateAsync(conversation);
        await unitOfWork.SaveChangesAsync();

        return conversation.Id;
    }

    public async Task DeleteConversation(Guid convId)
    {
        Conversation conversation = await conversationRepository.GetByIdAsync(convId) ??
            throw new Exception("Conversation with id : {id} not exist in db.");

        conversation.DeleteConversation();
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ConversationResponse> GetConversationByIdAsync(Guid convId)
    {
        Conversation conv = await conversationRepository.GetByIdAsync(convId) ??
            throw new Exception("Conversation with id : {id} not exist in db.");

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
}

