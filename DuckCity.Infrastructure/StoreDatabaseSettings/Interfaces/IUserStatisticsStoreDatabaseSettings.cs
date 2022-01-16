namespace DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

public interface IUserStatisticsStoreDatabaseSettings
{
    string? UserStatisticsCollectionName { get; set; }
    string? ConnectionString { get; set; }
    string? DatabaseName { get; set; }
}