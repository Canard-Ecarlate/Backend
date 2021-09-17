using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanardEcarlate.Domain.Database
{
    public class UserStatistics
    {
        [Key]
        public int UserName { get; set; }
        public int NbGamesPlayed { get; set; }
        public int NbWonAsCIAT { get; set; }
        public int NbLostAsCIAT { get; set; }
        public int NbWonAsCE { get; set; }
        public int NbLostAsCe { get; set; }
        public int Streak { get; set; }
        public int MaxWinStreak { get; set; }
        public int MaxLossStreak { get; set; }
        public List<CardsConfigurationStatistics> CardsConfigurationStatistics { get; set; }
    }
}
