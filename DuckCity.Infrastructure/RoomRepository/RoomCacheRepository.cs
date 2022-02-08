using DuckCity.Domain.Rooms;
using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.RoomRepository;

public class RoomCacheRepository : IRoomRepository
{
    private static readonly HashSet<Room> Rooms = new();
    public Room? FindRoomByRoomId(string roomId)
    {
        return Rooms.SingleOrDefault(g => g.RoomId == roomId);
    }
    
    private static Room? FindRoomByConnectionId(string connectionId)
    {
        return Rooms.SingleOrDefault(g => g.Players.SingleOrDefault(p => p.ConnectionId == connectionId) != null);
    }

    public void Add(Room newRoom)
    {
        Rooms.Add(newRoom);
    }

    public void Remove(Room room)
    {
        Rooms.Remove(room);
    }

    public Room SetPlayerReadyInRoom(string roomId, string connectionId)
    {
        Room room = FindRoomByRoomId(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.Ready = !player.Ready;
        return room;
    }

    public Room? DisconnectPlayerFromRoom(string connectionId)
    {
        Room? room = FindRoomByConnectionId(connectionId);
        if (room == null)
        {
            return room;
        }
        
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.ConnectionId = null;
        return room;
    }
}