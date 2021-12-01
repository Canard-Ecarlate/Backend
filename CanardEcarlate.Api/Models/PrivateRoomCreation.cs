using CanardEcarlate.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanardEcarlate.Api.Models
{
    public class PrivateRoomCreation
    {
        public string HostName { get; set; }
        public string RoomName { get; set; }
        public GameConfiguration gameConfiguration { get; set; }
    }
}
