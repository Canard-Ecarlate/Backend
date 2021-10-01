using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CanardEcarlate.Domain.Configuration
{
    class CardsConfigurationUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("userid")]
        public int UserId { get; set; }
        [BsonElement("cardsconfiguration")]
        public CardsConfiguration CardsConfiguration { get; set; }
    }
}
