using ChatApp.Application.Dtos.Auth;

namespace ChatApp.Application.Interfaces.Services;

public interface IAuthneticationService
{
   Task<AuthResponse> RegisterUserAsync(RegisterDto registerDto);
   Task<AuthResponse> LogInUserAsync(LoginDto loginDto);
}
