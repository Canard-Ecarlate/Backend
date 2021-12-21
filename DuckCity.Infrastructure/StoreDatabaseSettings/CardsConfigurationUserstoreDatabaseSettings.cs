using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

namespace DuckCity.Infrastructure.StoreDatabaseSettings
{
    public class CardsConfigurationUserStoreDatabaseSettings : ICardsConfigurationUserStoreDatabaseSettings
    {
        public string? CardsConfigurationUsersCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
