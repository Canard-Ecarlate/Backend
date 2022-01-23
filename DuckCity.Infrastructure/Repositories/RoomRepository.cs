using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories
{
    public class RoomRepository
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomRepository(IRoomStoreDatabaseSettings settings)
        {
            MongoClient client = new(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
            _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
        }

        public void Create(Room room)
        {
            _rooms.InsertOne(room);
        }
        
        public void Replace(Room room)
        {
            FilterDefinition<Room>? filter = Builders<Room>.Filter.Eq(r => r.Id, room.Id);
            _rooms.ReplaceOne(filter, room);
        }

        public long CountRoomById(string? id) =>
            _rooms.Find(room => room.Id == id).CountDocuments();

        public Room FindById(string? id)
        {
            Room room = _rooms.Find(room => room.Id == id).ToList().First();
            if (room == null)
            {
                throw new RoomNotFoundException();
            }
            return room;
        }

        public IEnumerable<Room> FindAllRooms() => 
            _rooms.Find(_ => true).ToList();

        public void Delete(Room room)
        {
            FilterDefinition<Room>? filter = Builders<Room>.Filter.Eq(r => r.Id, room.Id);
            _rooms.DeleteOne(filter);
        }
    }
}
