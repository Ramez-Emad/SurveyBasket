using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Service.Authentication;
public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    public (string token, int expiresIn) GenerateToken(ApplicationUser applicationUser ,
        IEnumerable<string> roles , IEnumerable<string> permissions)
    {
        Claim[] claims = [
            new (JwtRegisteredClaimNames.Sub, applicationUser.Id),
            new (JwtRegisteredClaimNames.Email, applicationUser.Email!),
            new (JwtRegisteredClaimNames.GivenName, applicationUser.FirstName),
            new (JwtRegisteredClaimNames.FamilyName, applicationUser.LastName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (nameof(roles) , JsonSerializer.Serialize(roles) , JsonClaimValueTypes.JsonArray ),
            new (nameof(permissions) , JsonSerializer.Serialize(permissions) , JsonClaimValueTypes.JsonArray ),

        ];


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

      
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );  

        return (
            new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            _options.ExpiryMinutes * 60
        );
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }
}
