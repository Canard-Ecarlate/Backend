using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("email")]
        public string? Email { get; set; }
        [BsonElement("password")]
        public string? Password { get; set; }
    }
}
