using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Mapping;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;
using TinyUrl.AuthenticationService.Infrastructure.Services;

namespace TinyUrl.AuthenticationService.Bussiness.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserContract> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false);

            return user is null ?
                throw new NotFoundException(ErrorMessages.UserNotFoundErrorMessage) :
                UserMapping.ToContract(user);
        }
    }
}
