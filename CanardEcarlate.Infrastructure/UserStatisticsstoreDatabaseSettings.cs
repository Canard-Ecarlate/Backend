namespace CanardEcarlate.Infrastructure
{
    public class UserStatisticsStoreDatabaseSettings : IUserStatisticsStoreDatabaseSettings
    {
        public string UserStatisticsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUserStatisticsStoreDatabaseSettings
    {
        string UserStatisticsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
