namespace ChatApp.Application.Dtos.Messages;

public class MessageDto
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

}
