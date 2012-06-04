using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace DataAccessLayer.Collections
{
    public static class SubjectsCollection
    {
        public static MongoCollection Collection
        {
            get
            {
                return TimrDatabase.Database.GetCollection("subjects");
            }
        }
    }
}
