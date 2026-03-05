using ChatApp.Application.Dtos.Auth;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
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

        // User return register after delete
        if (dbUser != null && dbUser.IsDeleted) {
            dbUser.UndoDeleteUser();
            await unitOfWork.SaveChangesAsync();
            (string jwtu, long expu) = jwtProvider.GenerateToken(dbUser);
            return new () {
                Token =  jwtu,
                TokenType = Enum.GetName(TokenType.Jwt)!,
                ExpiresIn = expu,
                UserId = dbUser.Id
            };
        }

        if (dbUser != null)
            throw new Exception("Email is already exists.");

        var passHash = passwordHasher.HashPasswords(registerDto.Password);

        var newUser = new User(Guid.NewGuid(),
                registerDto.Username,
                registerDto.Email,
                passHash);

        await userRepository.CreateUserAsync(newUser);
        await unitOfWork.SaveChangesAsync();

        (string jwt, long exp) = jwtProvider.GenerateToken(newUser);
        AuthResponse response = new () {
            Token =  jwt,
            TokenType = Enum.GetName(TokenType.Jwt)!,
            ExpiresIn = exp,
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

        (string jwt, long exp) = jwtProvider.GenerateToken(dbUser);
        AuthResponse response = new () {
            Token =  jwt,
            TokenType = Enum.GetName(TokenType.Jwt)!,
            ExpiresIn = exp,
            UserId = dbUser.Id
        };

        return response;
    }
}
