using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.AuthenticationService.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserContract> GetUserByIdAsync(int id);
    }
}
