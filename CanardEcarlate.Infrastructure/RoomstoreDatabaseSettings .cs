namespace CanardEcarlate.Infrastructure
{
    public class RoomStoreDatabaseSettings : IRoomStoreDatabaseSettings
    {
        public string RoomsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IRoomStoreDatabaseSettings
    {
        string RoomsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
