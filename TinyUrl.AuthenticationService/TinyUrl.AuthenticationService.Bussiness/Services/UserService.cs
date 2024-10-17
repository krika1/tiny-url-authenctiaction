using TinyUrl.AuthenticationService.Infrastructure.Clients;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
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
        private readonly IUrlClient _urlClient;
        private readonly IUserLimitRepository _userLimitRepository;

        public UserService(IUserRepository userRepository, IUrlClient urlClient, IUserLimitRepository userLimitRepository)
        {
            _userRepository = userRepository;
            _urlClient = urlClient;
            _userLimitRepository = userLimitRepository;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false)
                ?? throw new NotFoundException(ErrorMessages.UserNotFoundErrorMessage);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.Password = hashedPassword;

            await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        }

        public async Task<UserContract> GetUserByIdAsync(int id, string token)
        {
            var user = await _userRepository.GetUserByIdAsync(id).ConfigureAwait(false)
                ?? throw new NotFoundException(ErrorMessages.UserNotFoundErrorMessage);

            var userContract = UserMapping.ToContract(user);

            var urlCount = await _urlClient.GetUserUrlCountAsync(token).ConfigureAwait(false);

            userContract.UrlCounter.Count = urlCount;

            var urlUserLimits = await _userLimitRepository.GetUserLimitAsync(user.Id).ConfigureAwait(false);

            userContract.ApiCallsCounter.Count = urlUserLimits.DailyCalls;

            return userContract;
        }
    }
}
