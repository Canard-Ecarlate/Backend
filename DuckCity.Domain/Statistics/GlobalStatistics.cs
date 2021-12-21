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
        [BsonElement("nb_all_games")]
        public int NbAllGames { get; set; }
        [BsonElement("nb_replays")]
        public int NbReplays { get; set; }
        [BsonElement("nb_quit_mid_game")]
        public int NbQuitMidGame { get; set; }
        [BsonElement("nb_won_as_blue_by_nb_players")]
        public Dictionary<NbPlayersConfiguration, int>? NbWonAsBlueByNbPlayers { get; set; }
        [BsonElement("nb_won_as_red_by_nb_players")]
        public Dictionary<NbPlayersConfiguration, int>? NbWonAsRedByNbPlayers { get; set; }
    }
}
