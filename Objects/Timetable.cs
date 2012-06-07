using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Timetable : MongoDBObject
    {
        public string Faculty { get; set; }
        public string GroupId { get; set; }
        public List<TimetableItem> TimetableItems { get; set; }

        public Timetable()
        {
            Faculty = String.Empty;
            GroupId = String.Empty;
            TimetableItems = new List<TimetableItem>();
        }
    }
}
