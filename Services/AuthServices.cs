using cat_API.DB;
using cat_API.DTOs;
using cat_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cat_API.Services
{
    public interface IAuthService
    {
        Task<string?> Login(AuthDTO user);
    }
    public class AuthService : IAuthService
    {
        private readonly appDbContext _dbcontext;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<UserModel> _passwordHasher = new();

        public AuthService(appDbContext dbContext, IConfiguration config)
        {
            _dbcontext = dbContext;
            _configuration = config;
        }

        public async Task<string?> Login(AuthDTO user)
        {
            var dbuser = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (dbuser == null)
            {
                return null;
            }

            var passVerification = _passwordHasher.VerifyHashedPassword(dbuser, dbuser.Password, user.Password);

            if (passVerification == PasswordVerificationResult.Success)
            {
                return GenerateJwt(dbuser);
            }

            return null;
        }

        private string GenerateJwt(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var audiences = _configuration.GetSection("Jwt:Audiences").Get<string[]>();
            var mainAudience = audiences?.First();
            var issuers = _configuration.GetSection("Jwt:Issuers").Get<string[]>();
            var mainIssuer = issuers?.First();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                }),
                Issuer = mainIssuer,
                Audience = mainAudience,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
