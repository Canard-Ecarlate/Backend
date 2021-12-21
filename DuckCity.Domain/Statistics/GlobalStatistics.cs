using System.Text.Json.Serialization;
using DuckCity.Domain.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckCity.Domain.Statistics
{
    public class GlobalStatistics
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Id { get; set; }
        [BsonElement("nballgames")]
        public int NbAllGames { get; set; }
        [BsonElement("nbreplays")]
        public int NbReplays { get; set; }
        [BsonElement("nbquimidgame")]
        public int NbQuitMidGame { get; set; }
        [BsonElement("nbwonasciatbynbplayers")]
        public Dictionary<NbPlayersConfiguration, int>? NbWonAsBlueByNbPlayers { get; set; }
        [BsonElement("nbwonascebynbplayers")]
        public Dictionary<NbPlayersConfiguration, int>? NbWonAsRedByNbPlayers { get; set; }
    }
}
