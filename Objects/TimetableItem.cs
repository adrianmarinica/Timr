using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects
{
    public class TimetableItem
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ClassName { get; set; }
        public ClassType TypeOfClass { get; set; }
        public string Teacher { get; set; }
        public string RoomNumber { get; set; }
        public ClassFrequency Frequency { get; set; }
        public bool IsOptional
        {
            get
            {
                return (OptionalPackage != -1) ? true : false;
            }
        }
        public int OptionalPackage { get; set; }
        public DayOfWeek Day { get; set; }
        public string StudyGroup
        {
            get
            {
                if (StudyGroups != null)
                    return StudyGroups.ElementAt(0);
                return null;
            }
        }
        public List<string> StudyGroups { get; set; }
        public TimetableItem()
        {
            ClassName = String.Empty;
            RoomNumber = String.Empty;
            OptionalPackage = -1;
            StartTime = String.Empty;
            EndTime = String.Empty;
        }
    }
}
