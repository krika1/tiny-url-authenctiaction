namespace TinyUrl.AuthenticationService.Infrastructure.Common
{
    public static class ErrorMessages
    {
        public const string UserAlreadyExistsErrorMessage = "User with that username or email already exists.";
        public const string LoginFailedErrorMessage = "Username or password are not correct.";
        public const string UserNotFoundErrorMessage = "User not found.";
    }
}
