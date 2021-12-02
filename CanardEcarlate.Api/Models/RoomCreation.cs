using CanardEcarlate.Domain.Configuration;

namespace CanardEcarlate.Api.Models
{
    public class RoomCreation
    {
        public string HostName { get; set; }
        public string RoomName { get; set; }
        public GameConfiguration GameConfiguration { get; set; }
    }
}
