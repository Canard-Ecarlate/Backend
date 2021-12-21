using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

namespace DuckCity.Infrastructure.StoreDatabaseSettings
{
    public class GlobalStatisticsStoreDatabaseSettings : IGlobalStatisticsStoreDatabaseSettings
    {
        public string? GlobalStatisticsCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
