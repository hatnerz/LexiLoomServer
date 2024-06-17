using LexiLoom.Database;
using LexiLoom.DTO;
using LexiLoom.Exceptions;
using LexiLoom.Interfaces;
using LexiLoom.Models;
using LexiLoom.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LexiLoom.Services
{
    public class AuthService : ServiceBase, IAuthService
    {
        public AuthService(LexiLoomDbContext context) : base(context)
        { }

        public string GenerateJwtToken(int userId, string login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new byte[32];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(key);
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, login),
                    new Claim("userId", $"{userId}"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User?> AuthenticateUser(LoginModel userData)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userData.Username);
            if (foundUser == null)
            {
                throw new NotFoundException("User", "username");
            }

            if (HashUtil.VerifyPassword(userData.Password, foundUser.PasswordHash))
            {
                return foundUser;
            }

            return null;
        }

        public async Task<User> RegisterUser(RegisterModel userData)
        {
            var isUserFound = await _context.Users.AnyAsync(u => u.Username == userData.Username || u.Email == userData.Email);

            if (isUserFound)
                throw new ArgumentException("User with this username or email already exists", nameof(userData));

            User newUser = new User()
            {
                Email = userData.Email,
                Username = userData.Username,
                PasswordHash = HashUtil.HashPassword(userData.Password),
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
    }
}
