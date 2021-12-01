using System.Collections.Generic;
using CanardEcarlate.Domain.Games;

namespace CanardEcarlate.Infrastructure.Cache
{
    public static class Variables
    {
        public static List<Room> PublicRooms { get; set; } = new List<Room>();
        public static List<Room> PrivateRooms { get; set; } = new List<Room>();
    }
}