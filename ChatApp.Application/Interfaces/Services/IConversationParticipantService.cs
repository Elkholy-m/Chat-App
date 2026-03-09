namespace ChatApp.Application.Interfaces.Services;

public interface IConversationParticipantService
{
   Task AddUserToConversation (Guid convId, IList<Guid> userId);
}
