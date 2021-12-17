using CanardEcarlate.Domain.Games;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CanardEcarlate.Infrastructure.Repositories
{
    public class RoomRepository
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomRepository(IRoomStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
        }

        public void Create(Room room)
        {
            _rooms.InsertOne(room);
        }

        public long CountRoomById(string id) =>
            _rooms.Find(room => room.Id == id).CountDocuments();

        public IList<Room> FindById(string id) =>
            _rooms.Find(room => room.Id == id).ToList();

        public IEnumerable<Room> FindAllRooms() => 
            _rooms.Find(_ => true).ToList();
    }
}
