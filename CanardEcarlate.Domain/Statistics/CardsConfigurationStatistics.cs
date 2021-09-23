using System.ComponentModel.DataAnnotations;

namespace CanardEcarlate.Domain
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
