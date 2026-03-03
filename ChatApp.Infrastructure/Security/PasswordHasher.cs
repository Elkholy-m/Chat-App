using ChatApp.Application.Interfaces.Infrastructure;

namespace ChatApp.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPasswords(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        return hash;
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        var IsValidPass = BCrypt.Net.BCrypt.Verify(password, storedHash);
        return IsValidPass;
    }
}
