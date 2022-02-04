using DuckCity.Domain.Rooms;

namespace DuckCity.Infrastructure.Repositories
{
    public interface IRoomRepository
    {
        void Create(Room room);

        void Replace(Room room);

        Room FindById(string id);

        IEnumerable<Room> FindAllRooms();

        void Delete(Room room);
        Room JoinRoom(string roomId, string userId);
    }
}