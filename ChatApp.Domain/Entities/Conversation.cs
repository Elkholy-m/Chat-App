namespace ChatApp.Domain.Entities;

public class Conversation {
    public Conversation() { }

    public Conversation(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<ConversationParticipant> Participants { get; private set; } = [];

    public ICollection<Message> Messages { get; private set; } = [];

    public void AddParticipant(Guid userId)
    {
        if (HasParticipant(userId))
            throw new InvalidOperationException("User already in conversation.");

        Participants.Add(new ConversationParticipant(Id, userId));
    }

    public bool HasParticipant(Guid userId)
    {
        return Participants.Any(p => p.UserId == userId);
    }
}
