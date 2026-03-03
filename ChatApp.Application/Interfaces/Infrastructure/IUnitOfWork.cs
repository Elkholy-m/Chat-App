namespace ChatApp.Application.Interfaces.Infrastructure; 

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
