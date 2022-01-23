using System.Text.Json.Serialization;
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
        public int NbAllGames { get; set; }
        public int NbReplays { get; set; }
        public int NbQuitMidGame { get; set; }
        public List<NbWonByNbPlayers>? NbWonAsBlueByNbPlayers { get; set; }
        public List<NbWonByNbPlayers>? NbWonAsRedByNbPlayers { get; set; }
    }
}
