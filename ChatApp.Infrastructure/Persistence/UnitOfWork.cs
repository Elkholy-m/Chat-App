using ChatApp.Application.Interfaces.Infrastructure;

namespace ChatApp.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
