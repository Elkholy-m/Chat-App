using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<User?> GetUserByEmailAsync(string email);

    Task<User?> GetUserByUserNameAsync(string username);

    Task<IList<User>> SearchByUsernameAsync(string username);
}
