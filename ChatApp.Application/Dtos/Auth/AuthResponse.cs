using ChatApp.Domain.Enums;

namespace ChatApp.Application.Dtos.Auth;

public class AuthResponse
{
    public TokenType TokenType { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresIn { get; set; }

    public Guid UserId { get; set; }
}
