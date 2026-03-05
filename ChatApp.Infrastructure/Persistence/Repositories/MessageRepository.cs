using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence.Repositories;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    private readonly DbSet<Message> _messages = context.Set<Message>();

    public async Task<Message?> GetByIdAsync(Guid messageId) =>
        await _messages.FindAsync(messageId);

    public async Task CreateMessageAsync(Message message) =>
        await _messages.AddAsync(message);

    public async Task<IList<Message>> GetMessagesForConversationAsync(Guid conversationId, int pageSize, int pageNumber) => 
        await _messages
        .Where(m => m.ConversationId == conversationId)
        .OrderByDescending(m => m.SentAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
