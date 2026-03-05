namespace ChatApp.Application.Dtos.Users;

public class UserChangePasswordRequest
{
   public string OldPassword { get; set; } = null!;

   public string NewPassword { get; set; } = null!;
}
