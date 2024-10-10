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
        private readonly MongoDbOptions _options;

        public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
            _mongoDatabase = mongoClient.GetDatabase(_options.DatabaseName);
        }

        public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>(USER_COLLECTION);
    }
}
