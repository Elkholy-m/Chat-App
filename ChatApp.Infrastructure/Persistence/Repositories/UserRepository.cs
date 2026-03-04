using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<User> Users = context.Set<User>();

    public async Task<IList<User>?> SearchByUsernameAsync(string username) =>
        await Users.Where(u => u.UserName.Contains(username)).ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid userId) =>
        await Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
    
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await Users.SingleOrDefaultAsync(u => u.Email.Equals(email));

    public async Task CreateUserAsync(User user) => 
        await Users.AddAsync(user);

    public async Task DeleteUserAsync(User user) => 
        user.DeleteUser();

    public async Task UpdateUserUsernameAsync(Guid id, string newUsername)
    {
        User user = await Users.SingleOrDefaultAsync(u => u.Id.Equals(id)) ??
            throw new Exception($"User with id : `{id}` not found in db");

        user.ChangeUserName(newUsername);
    }

    public async Task UpdateUserPasswordAsync(Guid id, string newPassHash)
    {
        User user = await Users.SingleOrDefaultAsync(u => u.Id.Equals(id)) ??
            throw new Exception($"User with id : `{id}` not found in db");

        user.ChangePassword(newPassHash);
    }

    public async Task UpdateUserEmailAsync(Guid id, string newEmail)
    {
        User user = await Users.SingleOrDefaultAsync(u => u.Id.Equals(id)) ??
            throw new Exception($"User with id : `{id}` not found in db");

        user.ChangeEmail(newEmail);
    }
}
