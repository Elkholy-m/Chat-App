using ChatApp.Application.Dtos.Users;

namespace ChatApp.Application.Interfaces.Services;

public interface IUserServie
{
   Task<IList<UserResponse>> SearchForUsers(string username);

   Task<UserResponse> GetUserById(Guid id);

   Task SoftDeleteUserAsync(Guid id);

   Task UpdateUserLastSeenAsync(Guid id);

   Task ChangeUserPasswordAsync(Guid id, string prevPass, string newPass);

   Task UpdateUserProfileAsync(Guid id, UserProfileRequest profileRequest);
}
