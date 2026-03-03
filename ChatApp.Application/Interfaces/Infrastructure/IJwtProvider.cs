using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Infrastructure;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
