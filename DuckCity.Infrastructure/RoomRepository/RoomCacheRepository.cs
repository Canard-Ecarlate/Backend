using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomRepository;

public class RoomCacheRepository : IRoomRepository
{
    private static readonly HashSet<Room> Rooms = new();
    
    public void Create(Room newRoom)
    {
        Rooms.Add(newRoom);
    }

    public Room? FindById(string roomId)
    {
        return Rooms.SingleOrDefault(g => g.Id == roomId);
    }

    public Room? FindByConnectionId(string connectionId)
    {
        return Rooms.SingleOrDefault(g => g.Players.SingleOrDefault(p => p.ConnectionId == connectionId) != null);
    }

    public void Update(Room roomUpdated)
    {
        // Nothing to do here, already updated
    }

    public void Delete(Room room)
    {
        Rooms.Remove(room);
    }
}