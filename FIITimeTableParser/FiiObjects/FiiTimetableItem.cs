using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;

namespace FIITimetableParser.FiiObjects
{
    public class FiiTimetableItem
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ClassName { get; set; }
        public ClassType TypeOfClass { get; set; }
        public string TeacherName { get; set; }
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
        public FiiGroup StudyGroup
        {
            get
            {
                if (StudyGroups != null)
                    return StudyGroups.ElementAt(0);
                return null;
            }
        }
        public List<FiiGroup> StudyGroups { get; set; }
        public FiiTimetableItem()
        {
            ClassName = String.Empty;
            TeacherName = String.Empty;
            RoomNumber = String.Empty;
            Frequency = ClassFrequency.EveryWeek;
            StartTime = string.Empty;
            EndTime = String.Empty;
            OptionalPackage = -1;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} - {2}", ClassName, TeacherName, Enum.GetName(typeof(ClassType), TypeOfClass));
        }
    }
}