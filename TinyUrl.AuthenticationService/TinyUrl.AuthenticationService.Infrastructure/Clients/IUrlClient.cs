namespace TinyUrl.AuthenticationService.Infrastructure.Clients
{
    public interface IUrlClient
    {
        Task<int> GetUserUrlCountAsync(string token);
    }
}
