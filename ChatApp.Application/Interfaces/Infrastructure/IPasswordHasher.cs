namespace ChatApp.Application.Interfaces.Infrastructure;

public interface IPasswordHasher
{
    string HashPasswords(string password);
    bool VerifyPassword(string password, string storedHash);
}
