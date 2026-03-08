using System.Security.Claims;
using ChatApp.Application.Dtos.Conversation;
using ChatApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/conversations")]
public class ConversationsController(IConversationService conversationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetConversationsOfUser([FromQuery] Guid userId) {
        // Claim idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ??
        //     throw new Exception("Invalid token");
        //
        // Guid authId = Guid.Parse(idClaim.Value);
        // if (authId != userId)
        //     Forbid();

        IList<ConversationResponse> conversations = 
            await conversationService.GetConversationsForUserAsync(userId);

        return Ok(conversations);
    }

    [HttpGet("{convId:guid}", Name = "GetConvById")]
    public async Task<IActionResult> GetConversationByConvId(Guid convId) {
        ConversationResponse conversationResponse =
            await conversationService.GetConversationByIdAsync(convId);

        return Ok(conversationResponse);
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateConversation([FromBody] IList<Guid> particpantIds) {
        Guid convId = await conversationService.CreateConversationAsync(particpantIds);
        return CreatedAtRoute("GetConvById", new { convId }, convId);
    }

    [HttpDelete("{convId:guid}")]
    public async Task<IActionResult> DeleteConversation([FromRoute] Guid convId) {
        await conversationService.DeleteConversation(convId);
        return NoContent();
    }
}
