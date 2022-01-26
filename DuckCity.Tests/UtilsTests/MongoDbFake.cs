using System;
using DuckCity.Infrastructure;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DuckCity.Tests.UtilsTests
{
    public class MongoDbFake : IDisposable
    {
        public MongoDbFake()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("UtilsTests/appsettings.json")
                .Build();

            string connString = config.GetConnectionString("db");
            string usersCollectionName = config.GetConnectionString("UsersCollectionName");
            string roomsCollectionName = config.GetConnectionString("RoomsCollectionName");
            string dbName = $"test_db_{Guid.NewGuid()}";

            MongoSettings = new MongoDbSettings
            {
                ConnectionString = connString, DatabaseName = dbName, UsersCollectionName = usersCollectionName,
                RoomsCollectionName = roomsCollectionName
            };
        }

        public MongoDbSettings MongoSettings { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MongoClient client = new(MongoSettings.ConnectionString);
                client.DropDatabase(MongoSettings.DatabaseName);
            }
        }
    }
}