using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.RoomRepository;

public interface IRoomRepository
{
    void Create(Room newRoom);

    Room? FindById(string roomId);

    Room? FindByCode(string code);
    
    Room? FindByConnectionId(string connectionId);

    Room? FindByUserId(string userId);
    
    void Update(Room roomUpdated);

    void Delete(Room room);
}