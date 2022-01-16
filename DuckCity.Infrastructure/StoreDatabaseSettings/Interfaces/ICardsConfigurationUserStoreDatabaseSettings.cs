namespace DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces
{
    public interface ICardsConfigurationUserStoreDatabaseSettings
    {
        string? CardsConfigurationUsersCollectionName { get; set; }
        string? ConnectionString { get; set; }
        string? DatabaseName { get; set; }
    }
}