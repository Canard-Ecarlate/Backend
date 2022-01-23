using DuckCity.Domain;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.StoreDatabaseSettings;
using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;
using MongoDB.Driver;

namespace DuckCity.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IUserStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
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
    }
}
