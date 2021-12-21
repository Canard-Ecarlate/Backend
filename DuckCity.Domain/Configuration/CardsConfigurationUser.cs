using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Configuration
{
    public class CardsConfigurationUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("user_id")]
        public int UserId { get; set; }
        [BsonElement("cards_configuration")]
        public CardsConfiguration? CardsConfiguration { get; set; }
    }
}
