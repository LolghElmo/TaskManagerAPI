using IdenttiyService.Domain.Interfaces;
using IdenttiyService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdenttiyService.Infastrcuture.Services
{
    public class TokenService : ITokenService
    {
        public readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string GenerateToken(User user)
        {
            var secretKey = _config["AppSettings:TokenKey"] ?? throw new ArgumentNullException("JWT Secret Token is missing from the configuration.");

            // Create The Symmatric Security Key And Signing Credits
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken
                (
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
                );

            // Return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
