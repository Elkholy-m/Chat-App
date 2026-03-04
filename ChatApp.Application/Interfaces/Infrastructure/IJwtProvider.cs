using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Infrastructure;

public interface IJwtProvider
{
    (string jwt, long exp)  GenerateToken(User user);
}
