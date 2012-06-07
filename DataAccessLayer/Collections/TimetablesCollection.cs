using MongoDB.Driver;

namespace DataAccessLayer.Collections
{
    public static class TimetablesCollection
    {
        public static MongoCollection Collection
        {
            get
            {
                return TimrDatabase.Database.GetCollection("timetables");
            }
        }
    }
}