using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatApp.Application.Interfaces.Infrastructure;
using ChatApp.Domain.Configurations;
using ChatApp.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Infrastructure.Security;

public class JwtProvider(IOptionsSnapshot<JwtConfiguration> options) : IJwtProvider
{
    private readonly JwtConfiguration jwtConfiguration = options.Value;

    public string GenerateToken(User user)
    {
        var jwtPrivateKey = Environment.GetEnvironmentVariable("JwtKey") ??
            throw new Exception("CAN'T GET JWT ENVIRONMET KEY");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtPrivateKey));
        var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
        };


        var expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtConfiguration.EXPIRATION));
        var token = new JwtSecurityToken(
                issuer: jwtConfiguration.ISSER,
                audience: jwtConfiguration.AUDIENCE,
                claims: claims,
                expires: expires,
                signingCredentials: credintials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
