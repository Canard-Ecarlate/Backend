using System.ComponentModel.DataAnnotations;
using DuckCity.Domain.Configuration;

namespace DuckCity.Domain.Statistics
{
    public class CardsConfigurationStatistics
    {
        [Key]
        public string? Id { get; set; }
        public CardsConfiguration? CardsConfiguration { get; set; }
        public int NbWonAsBlue { get; set; }
        public int NbWonAsRed { get; set; }
    }
}
