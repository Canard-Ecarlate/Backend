namespace DuckCity.Infrastructure;

public interface IMongoDbSettings
{
    string? ConnectionString { get; set; }
    string? DatabaseName { get; set; }
    string? UsersCollectionName { get; set; }
    string? RoomsCollectionName { get; set; }
    string? GameContainersCollectionName { get; set; }
}