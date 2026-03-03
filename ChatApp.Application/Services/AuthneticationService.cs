using ChatApp.Application.Dtos.Auth;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services;

public class AuthneticationService(
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : IAuthneticationService
{
    public async Task<AuthResponse> RegisterUserAsync(RegisterDto registerDto)
    {
        User? dbUser = await userRepository.GetUserByEmailAsync(registerDto.Email);
        if (dbUser != null) {
            throw new Exception("Email is already exists.");
        }

        var passHash = passwordHasher.HashPasswords(registerDto.Password);

        var newUser = new User(Guid.NewGuid(),
                registerDto.Username,
                registerDto.Email,
                passHash);

        await userRepository.CreateUserAsync(newUser);
        await unitOfWork.SaveChangesAsync();

        string jwt = jwtProvider.GenerateToken(newUser);
        AuthResponse response = new () {
            Token =  jwt,
            TokenType = Domain.Enums.TokenType.Jwt,
            UserId = newUser.Id
        };

        return response;
    }

    public async Task<AuthResponse> LogInUserAsync(LoginDto loginDto)
    {
        User? dbUser = await userRepository.GetUserByEmailAsync(loginDto.Email) ??
            throw new Exception("INVALID CREDENTIALS");

        if (!passwordHasher.VerifyPassword(loginDto.Password, dbUser.PasswordHash))
            throw new Exception("INVALID CREDENTIALS");

        string jwt = jwtProvider.GenerateToken(dbUser);
        AuthResponse response = new () {
            Token =  jwt,
            TokenType = Domain.Enums.TokenType.Jwt,
            UserId = dbUser.Id
        };

        return response;
    }
}
