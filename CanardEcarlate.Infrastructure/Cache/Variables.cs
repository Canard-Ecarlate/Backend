using System.Collections.Generic;
using System.Collections.ObjectModel;
using CanardEcarlate.Domain.Games;

namespace CanardEcarlate.Infrastructure.Cache
{
    public static class Variables
    {
        public static Collection<Room> PublicRooms { get; } = new Collection<Room>();
        public static Collection<Room> PrivateRooms { get; } = new Collection<Room>();
    }
}