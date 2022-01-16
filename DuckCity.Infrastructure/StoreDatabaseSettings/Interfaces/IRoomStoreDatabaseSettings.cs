namespace DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

public interface IRoomStoreDatabaseSettings
{
    string? RoomsCollectionName { get; set; }
    string? ConnectionString { get; set; }
    string? DatabaseName { get; set; }
}