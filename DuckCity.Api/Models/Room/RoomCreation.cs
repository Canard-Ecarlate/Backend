using DuckCity.Domain.Configuration;

namespace DuckCity.Api.Models.Room
{
    public class RoomCreation
    {
        public string? HostId { get; set; }
        public string? RoomName { get; set; }
        public GameConfiguration? GameConfiguration { get; set; }
        public bool IsPrivate { get; set; }
    }
}
