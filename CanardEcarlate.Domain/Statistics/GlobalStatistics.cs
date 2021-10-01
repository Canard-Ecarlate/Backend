using System.Collections.Generic;
using System.Text.Json.Serialization;
using CanardEcarlate.Domain.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CanardEcarlate.Domain.Statistics
{
    public class GlobalStatistics
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }
        [BsonElement("nballgames")]
        public int NbAllGames { get; set; }
        [BsonElement("nbreplays")]
        public int NbReplays { get; set; }
        [BsonElement("nbquimidgame")]
        public int NbQuitMidGame { get; set; }
        [BsonElement("nbwonasciatbynbplayers")]
        public Dictionary<NbPlayersConfiguration, int> NbWonAsCIATByNbPlayers { get; set; }
        [BsonElement("nbwonascebynbplayers")]
        public Dictionary<NbPlayersConfiguration, int> NbWonAsCEByNbPlayers { get; set; }
    }
}
