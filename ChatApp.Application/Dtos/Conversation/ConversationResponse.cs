namespace ChatApp.Application.Dtos.Conversation;

public class ConversationResponse
{
   public Guid Id { get; set; } 

   public DateTime CreatedAt { get; set; } 

   public IList<Guid>? ParticipantsIds { get; set; } 
}
