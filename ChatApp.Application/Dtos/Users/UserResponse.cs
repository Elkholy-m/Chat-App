namespace ChatApp.Application.Dtos.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? LastSeenAt { get; set; }
}
