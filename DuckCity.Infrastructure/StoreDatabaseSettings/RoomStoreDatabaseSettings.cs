using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

namespace DuckCity.Infrastructure.StoreDatabaseSettings
{
    public class RoomStoreDatabaseSettings : IRoomStoreDatabaseSettings
    {
        public string? RoomsCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
