﻿using Microsoft.Extensions.Configuration;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTO;

namespace Infrastructure.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Token(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            
            var jwtKey = _configuration["JwtConfig:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public ClaimsPrincipal ValidateToken(TokenDTO token)
        {
            var tokenString = token.Token;
            if (string.IsNullOrEmpty(tokenString))
                throw new ArgumentException("Given token is null or empty.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                return tokenHandler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                
                throw new SecurityTokenException("Token validation failed.", ex);
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"])),
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = true, 
                ClockSkew = TimeSpan.Zero 
            };
        }
    }
}