using System.Text.Json.Serialization;
using DuckCity.Domain.Cards;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Users
{
    public class CardsConfigurationUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public List<NbEachCard> Cards { get; set; } = new();
    }
}
