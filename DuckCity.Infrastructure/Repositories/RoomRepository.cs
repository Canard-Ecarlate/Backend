using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly IMongoCollection<Room> _rooms;

    public RoomRepository(IMongoDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
        _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
    }

    public void Create(Room room) => _rooms.InsertOne(room);

    public void Replace(Room room) => _rooms.ReplaceOne(Builders<Room>.Filter.Eq(r => r.Id, room.Id), room);

    public Room FindById(string id)
    {
        try
        {
            return _rooms.Find(room => room.Id == id).First();
        }
        catch
        {
            throw new RoomNotFoundException(id);
        }
    }

    public IEnumerable<Room> FindAllRooms() => _rooms.Find(_ => true).ToList();

    public void Delete(Room room) => _rooms.DeleteOne(Builders<Room>.Filter.Eq(r => r.Id, room.Id));
}