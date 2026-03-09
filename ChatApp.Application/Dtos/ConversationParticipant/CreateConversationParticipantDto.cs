namespace ChatApp.Application.Dtos.ConversationParticipant;

public class CreateConversationParticipantDto
{
    public Guid ConversationId { get; private set; }

    public Guid UserId { get; private set; }

}
