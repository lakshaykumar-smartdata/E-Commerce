using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<string> UserRegistration(User user);
        Task<string> UserLogin(string email, string password);
    }
}
