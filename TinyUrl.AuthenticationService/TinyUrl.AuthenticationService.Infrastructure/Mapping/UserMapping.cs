using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Mapping
{
    public static class UserMapping
    {
        public static User ToDomain(RegisterUserRequest request)
        {
            return new User { Email = request.Email, Id = IdGenerator.GeneratateUserId(), Username = request.Username };
        }

        public static UserContract ToContract(User user)
        {
            return new UserContract { Email = user.Email, UserId = user.Id, Username = user.Username };
        }
    }
}
