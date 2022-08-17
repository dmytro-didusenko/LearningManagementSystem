using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Auth;
using LearningManagementSystem.Domain.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LearningManagementSystem.Core.Utils;

public class JwtHandler : IJwtHandler
{
    private readonly ILogger<JwtHandler> _logger;
    private readonly AppDbContext _db;
    private readonly JwtSettingsOptions _jwtSettings;

    public JwtHandler(IOptions<JwtSettingsOptions> jwtSettingsOptions,
        ILogger<JwtHandler> logger,
        AppDbContext db)
    {
        _logger = logger;
        _db = db;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role?.RoleName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public AuthUserModel? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.ValidAudience,
                ValidIssuer = _jwtSettings.ValidIssuer,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;

            var claimId = jwtToken?.Claims.FirstOrDefault(x => x.Type.Equals("nameid"));
            var validate = Guid.TryParse(claimId?.Value, out var userId);
            var userName = jwtToken?.Claims.FirstOrDefault(f => f.Type.Equals("name"))?.Value;
            var role = jwtToken?.Claims.FirstOrDefault(f => f.Type.Equals("role"))?.Value;

            if (!validate || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(role))
            {
                return null;
            }

            return new AuthUserModel()
            {
                Id = userId,
                Role = role,
                UserName = userName
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation($"--Error in validating: {e.Message}");
            return null;
        }
    }

    public RefreshTokenModel GenerateRefreshTokenModel(string ipAddress)
    {
        throw new NotImplementedException();
    }
}