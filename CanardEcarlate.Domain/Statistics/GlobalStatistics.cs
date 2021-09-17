using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanardEcarlate.Domain.Database
{
    public class GlobalStatistics
    {
        [Key]
        public int NbAllGames { get; set; }
        [Key]
        public int NbReplays { get; set; }
        [Key]
        public int NbQuitMidGame { get; set; }
        [Key]
        public Dictionary<NbPlayersConfiguration, int> NbWonAsCIATByNbPlayers { get; set; }
        [Key]
        public Dictionary<NbPlayersConfiguration, int> NbWonAsCEByNbPlayers { get; set; }
    }
}
