using System.ComponentModel.DataAnnotations;
using DuckCity.Domain.Cards;

namespace DuckCity.Domain.Statistics;

public class CardsConfigurationStatistics
{
    [Key]
    public string? Id { get; set; }
    public List<NbEachCard>? Cards { get; set; }
    public int NbWonAsBlue { get; set; }
    public int NbWonAsRed { get; set; }
}