using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class JwtTokenService
    {
        public string CreateToken(ApplicationUser user)
        {
            var jwtKey =
                Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new Exception("JWT_KEY is missing.");

            var jwtIssuer =
                Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? throw new Exception("JWT_ISSUER is missing.");

            var jwtAudience =
                Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                ?? throw new Exception("JWT_AUDIENCE is missing.");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("fullName", user.FullName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
