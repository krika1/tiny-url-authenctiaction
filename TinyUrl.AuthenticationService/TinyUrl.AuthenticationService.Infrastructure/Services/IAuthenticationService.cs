using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.AuthenticationService.Infrastructure.Services
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(RegisterUserRequest request);
        Task<TokenContract> LoginAsync(LoginRequest request);
    }
}
