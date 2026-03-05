using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<User> _users = context.Set<User>();

    public async Task<IList<User>> SearchByUsernameAsync(string username) =>
        await _users
            .AsNoTracking()
            .Where(u => !u.IsDeleted && u.UserName.Contains(username))
            .Take(20)
            .ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid userId) =>
        await _users.FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == userId);
    
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await _users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetUserByUserNameAsync(string username) =>
        await _users.FirstOrDefaultAsync(u => u.UserName == username);

    public async Task CreateUserAsync(User user) => 
        await _users.AddAsync(user);
}
