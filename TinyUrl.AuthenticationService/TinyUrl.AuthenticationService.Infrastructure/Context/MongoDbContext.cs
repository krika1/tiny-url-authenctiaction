using MongoDB.Driver;
using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private const string USER_COLLECTION = "Users";

        public MongoDbContext(IMongoClient mongoClient)
        {
            _mongoDatabase = mongoClient.GetDatabase("TinyUrlDatabase");
        }

        public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>(USER_COLLECTION);
    }
}
