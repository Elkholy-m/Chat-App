using Microsoft.AspNetCore.Mvc;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Dtos.Auth;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthneticationService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto) {
        var authResponse =  await authService.RegisterUserAsync(registerDto);
        return Ok(authResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto) {
        var authResponse =  await authService.LogInUserAsync(loginDto);
        return Ok(authResponse);
    }
}
