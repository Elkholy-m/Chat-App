namespace ChatApp.Domain.Entities;

public class ConversationParticipant
{
    private ConversationParticipant() { }

    public ConversationParticipant(Guid conversationId, Guid userId)
    {
        ConversationId = conversationId;
        UserId = userId;
        JoinedAt = DateTime.UtcNow;
    }

    public Guid ConversationId { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime JoinedAt { get; private set; }

    // Navigation properties
    public Conversation Conversation { get; private set; } = null!;

    public User User { get; private set; } = null!;
}
