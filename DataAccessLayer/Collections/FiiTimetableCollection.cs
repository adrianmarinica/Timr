using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace DataAccessLayer.Collections
{
    public class FiiTimetableCollection
    {
        public static MongoCollection Collection
        {
            get
            {
                return TimrDatabase.Database.GetCollection("fiitimetable");
            }
        }
    }
}
