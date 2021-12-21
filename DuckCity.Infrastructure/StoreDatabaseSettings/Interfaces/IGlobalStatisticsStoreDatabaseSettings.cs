namespace DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

public interface IGlobalStatisticsStoreDatabaseSettings
{
    string? GlobalStatisticsCollectionName { get; set; }
    string? ConnectionString { get; set; }
    string? DatabaseName { get; set; }
    
}