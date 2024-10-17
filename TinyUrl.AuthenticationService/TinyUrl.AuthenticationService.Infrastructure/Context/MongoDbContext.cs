using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Options;
using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private const string USER_COLLECTION = "Users";
        private const string USERS_LIMIT_COLLECTION = "UsersLimit";
        private readonly MongoDbOptions _options;

        public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
            _mongoDatabase = mongoClient.GetDatabase(_options.DatabaseName);
        }

        public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>(USER_COLLECTION);
        public IMongoCollection<UserLimit> UsersLimits => _mongoDatabase.GetCollection<UserLimit>(USERS_LIMIT_COLLECTION);
    }
}
