namespace TinyUrl.AuthenticationService.Infrastructure.Contracts.Options
{
    public class MongoDbOptions
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
