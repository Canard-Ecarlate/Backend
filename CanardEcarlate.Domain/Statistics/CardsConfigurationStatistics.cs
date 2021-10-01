using System.ComponentModel.DataAnnotations;
using CanardEcarlate.Domain.Configuration;

namespace CanardEcarlate.Domain.Statistics
{
    public class CardsConfigurationStatistics
    {
        [Key]
        public int Id { get; set; }
        public CardsConfiguration CardsConfiguration { get; set; }
        public int NbWonAsCIAT { get; set; }
        public int NbWonAsCE { get; set; }
    }
}
