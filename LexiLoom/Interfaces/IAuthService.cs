using LexiLoom.DTO;
using LexiLoom.Models;

namespace LexiLoom.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterUser(RegisterModel userData);

        Task<User?> AuthenticateUser(LoginModel userData);

        string GenerateJwtToken(int userId, string login);
    }
}
