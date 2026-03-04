namespace ChatApp.Application.Dtos.Auth;

public class AuthResponse
{
    public string TokenType { get; set; } = null!;

    public string Token { get; set; } = null!;

    public long ExpiresIn { get; set; }

    public Guid UserId { get; set; }
}
