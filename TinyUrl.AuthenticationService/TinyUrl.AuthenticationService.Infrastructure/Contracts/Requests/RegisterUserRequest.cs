using TinyUrl.AuthenticationService.Infrastructure.Attributes;

namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests
{
    public class RegisterUserRequest
    {
        public string? Username { get; set; }
        [EmailValidation(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
