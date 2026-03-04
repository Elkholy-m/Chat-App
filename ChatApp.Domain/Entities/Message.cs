namespace ChatApp.Domain.Entities;

public class Message
{
    private Message() { }

    public Message(Guid id, Guid conversationId, Guid senderId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Message content cannot be empty.");

        if (content.Length > 4000)
            throw new ArgumentException("Message exceeds maximum allowed length.");

        Id = id;
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        SentAt = DateTime.UtcNow;
        IsRead = false;
    }

    public Guid Id { get; private set; }

    public Guid ConversationId { get; private set; }

    public Guid SenderId { get; private set; }

    public string Content { get; private set; } = null!;

    public DateTime SentAt { get; private set; }

    public bool IsRead { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public virtual Conversation Conversation { get; private set; } = null!;

    public virtual User Sender { get; private set; } = null!;


    // Domain behavior

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void Delete()
    {
        if (!IsDeleted) {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
