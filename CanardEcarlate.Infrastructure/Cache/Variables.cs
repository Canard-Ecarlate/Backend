using System.Collections.Generic;
using CanardEcarlate.Domain.Games;

namespace CanardEcarlate.Infrastructure.Cache
{
    public static class Variables
    {
        public static List<Room> Rooms { get; set; } = new List<Room>();
    }
}