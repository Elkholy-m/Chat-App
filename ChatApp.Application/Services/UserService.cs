using ChatApp.Application.Dtos.Users;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services;

public class UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHaser,
        IUnitOfWork unitOfWork) : IUserServie
{
    public async Task UpdateUserProfileAsync(Guid id, UserProfileRequest profileRequest) {
        User user = await userRepository.GetUserByIdAsync(id) ??
            throw new Exception($"User with id : {id} not found in db");

        if (profileRequest.UserName != null) {
            User? dbUsername = await userRepository.GetUserByUserNameAsync(profileRequest.UserName);
            if (dbUsername != null)
                throw new Exception($"Username is already exists.");

            user.ChangeUserName(profileRequest.UserName);
        }


        if (profileRequest.Email != null) {
            User? dbEmail = await userRepository.GetUserByEmailAsync(profileRequest.Email);
            if (dbEmail != null)
                throw new Exception($"Email is already exists.");

            user.ChangeEmail(profileRequest.Email);
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserLastSeenAsync(Guid id)
    {
        User user = await userRepository.GetUserByIdAsync(id) ??
            throw new Exception($"User with id : {id} not found in db");

        user.UpdateLastSeen();
        await unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeUserPasswordAsync(Guid id, string prevPass, string newPass)
    {
        User user = await userRepository.GetUserByIdAsync(id) ??
            throw new Exception($"User with id : {id} not found in db");

        if (!passwordHaser.VerifyPassword(prevPass, user.PasswordHash))
            throw new Exception($"INVALID CREDENTIALS");

        var newPassHash = passwordHaser.HashPasswords(newPass);
        user.ChangePassword(newPassHash);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<UserResponse> GetUserById(Guid id)
    {
        User user = await userRepository.GetUserByIdAsync(id) ??
            throw new Exception($"User with id : {id} not found in db");

             UserResponse userResponse  = new() {
                 Id = user.Id,
                 UserName = user.UserName,
                 Email = user.Email,
                 LastSeenAt = user.LastSeenAt
             };

             return  userResponse;
    }

    public async Task<IList<UserResponse>> SearchForUsers(string username)
    {
         IList<User> users = await userRepository.SearchByUsernameAsync(username);

         IList<UserResponse> userResponses = [];

         // Mapping
         foreach (User user in users) {
             UserResponse userResponse  = new() {
                 Id = user.Id,
                 UserName = user.UserName,
                 Email = user.Email,
                 LastSeenAt = user.LastSeenAt
             };

             userResponses.Add(userResponse);
         }

         return userResponses;
    }

    public async Task SoftDeleteUserAsync(Guid id)
    {
        User user = await userRepository.GetUserByIdAsync(id) ??
            throw new Exception($"User with id : {id} not found in db");

        user.DeleteUser();
        await unitOfWork.SaveChangesAsync();
    }
}
