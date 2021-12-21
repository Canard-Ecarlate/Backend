using DuckCity.Domain.Configuration;

namespace DuckCity.Domain.Games
{
    public class Room
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? HostId { get; set; }
        public string? HostName { get; set; }
        public GameConfiguration? GameConfiguration { get; set; }
        public HashSet<PlayerInRoom>? Players { get; set; }
        public DataInGame? DataInGame { get; set; }
        public bool IsPrivate { get; set; }
        public string? ContainerId { get; set; }
    }
}
