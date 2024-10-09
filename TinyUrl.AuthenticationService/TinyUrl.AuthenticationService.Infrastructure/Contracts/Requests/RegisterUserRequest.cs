namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests
{
    public class RegisterUserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
