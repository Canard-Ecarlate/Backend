using CanardEcarlate.Domain.Database;
using CanardEcarlate.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CanardEcarlate.Application
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IUserstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            user.Password = savedPasswordHash;
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);

        public User Login(String name, String password){
            long nbPseudo = _users.Find(user => user.Name == name).CountDocuments();
            if (nbPseudo == 1)
            {
                User usertemp = _users.Find(user => user.Name == name).First();
                /* Extract the bytes */
                byte[] hashBytes = Convert.FromBase64String(usertemp.Password);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                        throw new UnauthorizedAccessException();

                return usertemp;
            }
            else {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
