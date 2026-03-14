using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VoiceChat.Api.Services;

public interface IAuthService
{
    string GenerateToken(string userId);
    string ValidateToken(string token);
}

public class AuthService : IAuthService
{
    private readonly string _secretKey;

    public AuthService(IConfiguration config)
    {
        _secretKey = config["Auth:SecretKey"] ?? "super-secret-key-minimum-32-characters-long!!!";
    }

    public string GenerateToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userId)
        };

        var token = new JwtSecurityToken(
            issuer: "CommunicationService",
            audience: "CommunicationService",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string ValidateToken(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var handler = new JwtSecurityTokenHandler();

            var principal = handler.ValidateToken(token.Replace("Bearer ",""), new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? throw new Exception("Invalid token");
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException("Token validation failed");
        }
    }
}


