using System.Collections.Generic;
using CanardEcarlate.Domain;
using MongoDB.Driver;

namespace CanardEcarlate.Infrastructure.Repositories
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

        public IList<User> GetByName(string name) =>
            _users.Find(user => user.Name == name).ToList();
        
        public void Create(User user)
        {
            _users.InsertOne(user);
        }

        public long CountUserByName(string name) =>
            _users.Find(user => user.Name == name).CountDocuments();

        public long CountUserByEmail(string email) =>
            _users.Find(user => user.Email == email).CountDocuments();
    }
}
