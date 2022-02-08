using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomRepository;

public interface IRoomRepository
{
    Room? FindRoomByRoomId(string gameId);
    
    void Add(Room newRoom);
    
    void Remove(Room room);
    
    Room SetPlayerReadyInRoom(string roomId, string connectionId);
    
    Room? DisconnectPlayerFromRoom(string connectionId);
}