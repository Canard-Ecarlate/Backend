using DuckCity.Domain.Users;

namespace DuckCity.Infrastructure.Repositories.Room;

public class RoomCacheRepository : IRoomRepository
{
    private static readonly HashSet<Domain.Rooms.Room> Rooms = new();
    public Domain.Rooms.Room? FindRoomByRoomId(string roomId)
    {
        return Rooms.SingleOrDefault(g => g.RoomId == roomId);
    }
    
    private static Domain.Rooms.Room? FindRoomByConnectionId(string connectionId)
    {
        return Rooms.SingleOrDefault(g => g.Players.SingleOrDefault(p => p.ConnectionId == connectionId) != null);
    }

    public void Add(Domain.Rooms.Room newRoom)
    {
        Rooms.Add(newRoom);
    }

    public void Remove(Domain.Rooms.Room room)
    {
        Rooms.Remove(room);
    }

    public Domain.Rooms.Room SetPlayerReadyInRoom(string roomId, string connectionId)
    {
        Domain.Rooms.Room room = FindRoomByRoomId(roomId)!;
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.Ready = !player.Ready;
        return room;
    }

    public Domain.Rooms.Room? DisconnectPlayerFromRoom(string connectionId)
    {
        Domain.Rooms.Room? room = FindRoomByConnectionId(connectionId);
        if (room == null)
        {
            return room;
        }
        
        Player player = room.Players.Single(p => p.ConnectionId == connectionId);
        player.ConnectionId = null;
        return room;
    }
}