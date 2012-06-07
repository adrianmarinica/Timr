using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;
using Objects;

namespace DataAccessLayer
{
    public class TimetablesDAL
    {
        public void InsertTimetable(Timetable timetable)
        {
            TimetablesCollection.Collection.Insert(timetable);
        }

        public List<TimetableItem> GetTimetableForFinalGroup(string groupId, string faculty)
        {
            var list = new List<TimetableItem>();
            var groupQuery = Query.EQ("GroupId", groupId);
            var facultyQuery = Query.EQ("Faculty", faculty);
            var both = Query.And(groupQuery, facultyQuery);
            var timetable = TimetablesCollection.Collection.FindOneAs<Timetable>(both);

            if (timetable != null && timetable.TimetableItems != null) 
                return timetable.TimetableItems;
            return null;
        }

        public void SaveTimetable(Timetable timetable)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTimetableItem(string timetableId, string groupId)
        {
            TimetablesCollection.Collection.FindOneByIdAs<Timetable>(timetableId);
        }
    }
}