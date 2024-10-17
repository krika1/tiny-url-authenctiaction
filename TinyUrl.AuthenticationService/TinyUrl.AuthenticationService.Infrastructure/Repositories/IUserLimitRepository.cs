using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Repositories
{
    public interface IUserLimitRepository
    {
        Task CrateRecordAsync(UserLimit userLimit);
        Task<UserLimit> GetUserLimitAsync(int userId);
    }
}
