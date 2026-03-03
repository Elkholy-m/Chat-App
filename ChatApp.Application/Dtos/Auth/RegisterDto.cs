using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Dtos.Auth;

public class RegisterDto
{
    [StringLength(30, MinimumLength = 7, ErrorMessage = "Username length must be between 7 and 30")]
    public required string Username { get; set; }

    [StringLength(30, MinimumLength = 7, ErrorMessage = "Email length must be between 7 and 30")]
    public required string Email { get; set; }

    [StringLength(30, MinimumLength = 7, ErrorMessage = "Password length must be between 7 and 30")]
    public required string Password { get; set; }
}
