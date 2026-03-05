using System.Security.Claims;
using ChatApp.Application.Dtos.Users;
using ChatApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserServie userServie) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string username) {
        IList<UserResponse> userResponses = await userServie.SearchForUsers(username);
        return Ok(userResponses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id) {
        UserResponse userResponse = await userServie.GetUserById(id);
        return Ok(userResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id) {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (userId == null || id != Guid.Parse(userId))
            Forbid();

        await userServie.SoftDeleteUserAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:guid}/profile")]
    public async Task<IActionResult> UpdateUserProfile([FromRoute] Guid id,
            [FromBody] UserProfileRequest profileRequest)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (userId == null || id != Guid.Parse(userId))
            Forbid();

        await userServie.UpdateUserProfileAsync(id, profileRequest);
        return NoContent();
    }

    [HttpPatch("{id:guid}/password")]
    public async Task<IActionResult> UpdateUserPassword([FromRoute] Guid id,
            [FromBody] UserChangePasswordRequest passwordRequest)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        if (userId == null || id != Guid.Parse(userId))
            Forbid();

        await userServie.ChangeUserPasswordAsync(id,
                passwordRequest.OldPassword,
                passwordRequest.NewPassword);
        return NoContent();
    }
}
