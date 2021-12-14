using CanardEcarlate.Domain.Configuration;

namespace CanardEcarlate.Api.Models
{
    public class RoomCreation
    {
        public string HostId { get; set; }
        public string RoomName { get; set; }
        public GameConfiguration GameConfiguration { get; set; }
        public bool IsPrivate { get; set; }
    }
}
