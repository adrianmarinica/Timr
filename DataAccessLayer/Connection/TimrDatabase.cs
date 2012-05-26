using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace DataAccessLayer
{
    public static class TimrDatabase
    {
        public static MongoDatabase Database
        {
            get
            {
                MongoServer server = MongoServer.Create(ConnectionSettings.ConnectionString);
                MongoDatabase database;
                if (!server.DatabaseExists("devtimr"))
                {
                    MongoDatabaseSettings settings = server.CreateDatabaseSettings("devtimr");
                    database = new MongoDatabase(server, settings);
                }
                else
                {
                    database = server.GetDatabase("devtimr");
                }
                return database;
            }
        }
    }
}
