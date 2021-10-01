namespace CanardEcarlate.Infrastructure
{
    public class GlobalStatisticsStoreDatabaseSettings : IGlobalStatisticsStoreDatabaseSettings
    {
        public string GlobalStatisticsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IGlobalStatisticsStoreDatabaseSettings
    {
        string GlobalStatisticsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
