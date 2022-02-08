using DuckCity.Domain.Users;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories.MongoImpl
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDbSettings settings)
        {
            MongoClient client = new(settings.ConnectionString);
            IMongoDatabase? database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public IList<User> GetByName(string? name) =>
            _users.Find(user => user.Name == name).ToList();

        public IList<User> GetById(string? id) =>
            _users.Find(user => user.Id == id).ToList();

        public void Create(User user)
        {
            _users.InsertOne(user);
        }

        public long CountUserByName(string? name) =>
            _users.Find(user => user.Name == name).CountDocuments();

        public long CountUserById(string? id) =>
            _users.Find(user => user.Id == id).CountDocuments();

        public long CountUserByEmail(string? email) =>
            _users.Find(user => user.Email == email).CountDocuments();

        public void DeleteUserById(string? id) =>
            _users.DeleteOne(user => user.Id == id);
        
        public void Replace(User user) => _users.ReplaceOne(Builders<User>.Filter.Eq(u => u.Id, user.Id), user);
    }
}