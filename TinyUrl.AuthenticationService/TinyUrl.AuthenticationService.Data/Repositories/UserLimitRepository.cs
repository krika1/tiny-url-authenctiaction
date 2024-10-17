using MongoDB.Driver;
using TinyUrl.AuthenticationService.Infrastructure.Context;
using TinyUrl.AuthenticationService.Infrastructure.Entities;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;

namespace TinyUrl.AuthenticationService.Data.Repositories
{
    public class UserLimitRepository : IUserLimitRepository
    {
        private readonly MongoDbContext _dbContext;

        public UserLimitRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CrateRecordAsync(UserLimit userLimit)
        {
            await _dbContext.UsersLimits.InsertOneAsync(userLimit);
        }

        public async Task<UserLimit> GetUserLimitAsync(int userId)
        {
            return await _dbContext.UsersLimits.Find(us => us.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
