namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses
{
    public class CounterContract
    {
        public int Count { get; set; }
        public int Limit { get; set; } = 100;
    }
}
