using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<User?> GetUserByEmailAsync(string email);

    Task<IList<User>?> SearchByUsernameAsync(string username);

    Task UpdateUserUsernameAsync(Guid id, string newUsername);

    Task UpdateUserPasswordAsync(Guid id, string newPassHash);

    Task UpdateUserEmailAsync(Guid id, string newEmail);

    Task DeleteUserAsync(User user);
}
