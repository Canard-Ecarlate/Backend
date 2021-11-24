using CanardEcarlate.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanardEcarlate.Api.Models
{
    public class UserJoinRoom
    {
        public string UserName { get; set; }
        public string RoomName { get; set; }
    }
}
