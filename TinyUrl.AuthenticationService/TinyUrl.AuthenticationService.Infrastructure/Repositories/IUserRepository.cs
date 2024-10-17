using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> CheckIfUserExistsAsync(string userName, string email);
    }
}
