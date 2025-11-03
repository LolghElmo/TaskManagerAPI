using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;


        public TokenService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }


        public string CreateToken(ApplicationUser user)
        {
            var secretKey = _config["AppSettings:TokenKey"] ?? throw new ArgumentNullException("JWT Secret Token is missing from the configuration.");

            // Create The Symmatric Security Key And Signing Credits
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken
                (
                issuer:null,
                audience:null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
                );

            // Return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
