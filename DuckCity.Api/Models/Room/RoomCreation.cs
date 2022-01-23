namespace DuckCity.Api.Models.Room
{
    public class RoomCreation
    {
        public string? Name { get; set; }
        public string? HostId { get; set; }
        public string? HostName { get; set; }
        public bool IsPrivate { get; set; }
        public int NbPlayers { get; set; }
    }
}
