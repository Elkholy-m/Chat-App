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

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public virtual ICollection<ConversationParticipant> ConversationParticipants { get; private set; } = [];

    public virtual ICollection<Message> Messages { get; private set; } = [];

    public void AddParticipant(Guid userId)
    {
        if (HasParticipant(userId))
            throw new InvalidOperationException("User already in conversation.");

        ConversationParticipants.Add(new ConversationParticipant(Id, userId));
    }

    public void RestoreParticipant(Guid userId)
    {
        var cp = ConversationParticipants.FirstOrDefault(cp => cp.UserId == userId) ??
            throw new InvalidOperationException("User is not in the conversation.");

        cp.RestoreParticipant();
    }

    public bool HasParticipant(Guid userId)
    {
        return ConversationParticipants.Any(p => p.UserId == userId);
    }

    public void DeleteConversation() {
        if (! IsDeleted) {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;

            foreach (var cp in ConversationParticipants)
                cp.DeleteParticipant();
        }
    }
}
