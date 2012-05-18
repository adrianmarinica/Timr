using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class TimetableItem
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
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
        public Group StudyGroup
        {
            get
            {
                if (StudyGroups != null)
                    return StudyGroups.ElementAt(0);
                return null;
            }
        }
        public List<Group> StudyGroups { get; set; }
        public TimetableItem()
        {
            ClassName = String.Empty;
            TeacherName = String.Empty;
            RoomNumber = String.Empty;
            Frequency = ClassFrequency.EveryWeek;
            OptionalPackage = -1;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} - {2}", ClassName, TeacherName, Enum.GetName(typeof(ClassType), TypeOfClass));
        }
    }
}
