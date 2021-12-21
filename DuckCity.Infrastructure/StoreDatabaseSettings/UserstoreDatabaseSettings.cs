using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;

namespace DuckCity.Infrastructure.StoreDatabaseSettings
{
    public class UserStoreDatabaseSettings : IUserStoreDatabaseSettings
    {
        public string? UsersCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
