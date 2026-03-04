namespace ChatApp.Domain.Entities;

public class User
{
    private User() { } // Required for EF Core

    public User(Guid id, string userName, string email, string passwordHash)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string UserName { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string PasswordHash { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    public DateTime? LastSeenAt { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public virtual ICollection<ConversationParticipant> ConversationParticipants { get; private set; } = [];

    public virtual ICollection<Message> Messages { get; private set; } = [];


    // Domain behavior methods

    public void UpdateLastSeen()
    {
        LastSeenAt = DateTime.UtcNow;
    }

    public void ChangeUserName(string newUserName)
    {
        if (string.IsNullOrWhiteSpace(newUserName))
            throw new ArgumentException("Username cannot be empty.");

        UserName = newUserName;
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty.");

        Email = newEmail;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Invalid password hash.");

        PasswordHash = newPasswordHash;
    }

    public void DeleteUser()
    {
        if (! IsDeleted) {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
