namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses
{
    public class UserContract
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        public CounterContract UrlCounter { get; set; } = new();
        public CounterContract ApiCallsCounter { get; set; } = new();
    }
}
