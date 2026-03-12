using System.Security.Claims;
using ChatApp.Application.Dtos.Messages;
using ChatApp.Application.Exceptions;
using ChatApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/conversations/{convId:guid}/messages")]
[Authorize]
public class MessagesController(IMessageService messageService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetConversationMessages([FromRoute] Guid convId,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize) 
    {
        // GET USER ID FROM CLAIM
        Guid userId = GetUserIdFromToken();

        IList<MessageDto> messagesDtos = await messageService.GetMessageForConversation(convId,
                userId, pageSize, pageNumber);

        return Ok(messagesDtos);
    }

    [HttpGet("{messageId:guid}")]
    public async Task<IActionResult> GetSpecificMessage(
            [FromRoute] Guid convId,
            [FromRoute] Guid messageId)
    {
         Claim idClaim  = User.FindFirst(ClaimTypes.NameIdentifier) ??
             throw new BadRequestException("INVALID JWT TOKEN");
        Guid userId = Guid.Parse(idClaim.Value);

        MessageDto messageDto = await messageService.GetMessageById(convId, userId, messageId);
        return Ok(messageDto);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToUser(
            [FromRoute] Guid convId,
            [FromBody] SendMessageDto sendMessageDto)
    {
        Guid userId = GetUserIdFromToken();

        await messageService.CreateMessage(convId, userId, sendMessageDto);
        return Created();
    }

    [HttpPatch("{messageId:guid}")]
    public async Task<IActionResult> ModifyMessageContent(
            [FromRoute] Guid convId,
            [FromRoute] Guid messageId,
            [FromBody] SendMessageDto sendMessageDto)
    {
        Guid userId = GetUserIdFromToken();

        await messageService.ChangeMessageContent(convId, userId, messageId, sendMessageDto.Content);
        return NoContent();
    }

    [HttpDelete("{messageId:guid}")]
    public async Task<IActionResult> DeletMessage(
            [FromRoute] Guid convId,
            [FromRoute] Guid messageId)
    {
        Guid userId = GetUserIdFromToken();

        await messageService.DeleteMessage(convId, userId, messageId);
        return NoContent();
    }

    private Guid GetUserIdFromToken() {
        Claim idClaim  = User.FindFirst(ClaimTypes.NameIdentifier) ??
            throw new BadRequestException("INVALID JWT TOKEN");

        Guid userId = Guid.Parse(idClaim.Value);
        return userId;
    }
}
