using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

namespace DuckCity.Infrastructure.StoreDatabaseSettings
{
    public class UserStatisticsStoreDatabaseSettings : IUserStatisticsStoreDatabaseSettings
    {
        public string? UserStatisticsCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
