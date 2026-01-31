using Prog2WebApi.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Prog2WebApi.Services
{
    public class AuthenticationService(IConfiguration configuration)
    {
        public string CreateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "30");

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
