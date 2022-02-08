namespace DuckCity.Infrastructure.Repositories.Room;

public interface IRoomRepository
{
    Domain.Rooms.Room? FindRoomByRoomId(string gameId);
    
    void Add(Domain.Rooms.Room newRoom);
    
    void Remove(Domain.Rooms.Room room);
    
    Domain.Rooms.Room SetPlayerReadyInRoom(string roomId, string connectionId);
    
    Domain.Rooms.Room? DisconnectPlayerFromRoom(string connectionId);
}