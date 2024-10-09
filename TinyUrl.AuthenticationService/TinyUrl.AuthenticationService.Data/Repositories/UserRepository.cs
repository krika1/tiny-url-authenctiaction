using MongoDB.Driver;
using TinyUrl.AuthenticationService.Infrastructure.Context;
using TinyUrl.AuthenticationService.Infrastructure.Entities;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;

namespace TinyUrl.AuthenticationService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbContext _dbContext;

        public UserRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckIfUserExistsAsync(string userName, string email)
        {
            var isUserExist = await _dbContext.Users.Find(u => u.Username == userName || u.Email == email).FirstOrDefaultAsync();

            return isUserExist is not null;
        }

        public async Task CreateUserAsync(User user)
        {
            await _dbContext.Users.InsertOneAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();
        }
    }
}
