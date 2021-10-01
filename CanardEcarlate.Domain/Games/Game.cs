using System.Collections.Generic;
using System.Text.Json.Serialization;
using CanardEcarlate.Domain.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CanardEcarlate.Domain.Games
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hostname")]
        public string HostName { get; set; }
        [BsonElement("gameconfiguration")]
        public GameConfiguration GameConfiguration { get; set; }
        [BsonElement("players")]
        public List<Player> Players { get; set; }
        [BsonElement("ingamedata")]
        public InGameData InGameData { get; set; }
    }
}
