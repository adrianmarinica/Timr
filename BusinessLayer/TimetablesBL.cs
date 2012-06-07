using System.Collections.Generic;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class TimetablesBL
    {
        private TimetablesDAL dal
        {
            get
            {
                return new TimetablesDAL();
            }
        }

        public List<TimetableItem> GetTimetableForGroup(string group, string faculty)
        {
            return dal.GetTimetableForFinalGroup(group, faculty);
        }

        public void SaveTimetable(Timetable timetable)
        {
            dal.InsertTimetable(timetable);
        }

        public void DeleteTimetableItem(string timetableId, string groupId)
        {
            dal.DeleteTimetableItem(timetableId, groupId);
        }
    }
}