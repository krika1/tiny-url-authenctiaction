using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TinyUrl.AuthenticationService.Infrastructure.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
