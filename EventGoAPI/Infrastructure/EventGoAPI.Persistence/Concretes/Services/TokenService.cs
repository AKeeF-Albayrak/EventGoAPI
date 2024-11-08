using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Enums;

namespace EventGoAPI.Persistence.Concretes.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _key;
        private readonly HashSet<string> _blacklistedTokens = new();

        public TokenService(IConfiguration configuration)
        {
            _key = configuration["JWT:Secret"];
            if (string.IsNullOrEmpty(_key))
            {
                throw new Exception("Secret key is missing or empty. Please check your configuration.");
            }
        }

        public string GenerateToken(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role == UserRole.Admin ? "admin" : "user")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task AddToBlacklistAsync(string token)
        {
            _blacklistedTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return Task.FromResult(_blacklistedTokens.Contains(token));
        }
    }
}
