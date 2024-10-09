namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses
{
    public class TokenContract
    {
        public string? AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
