using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Objects;

namespace FIITimetableParser
{
    public class Exporter
    {
        public string ConvertToXML(List<TimetableItem> timetable)
        {
            if (timetable != null)
            {
                return GetXMLFromTimetable(timetable).ToString();
            }
            return String.Empty;
        }

        public XDocument ConvertToXDocument(List<TimetableItem> timetable)
        {
            if (timetable != null)
            {
                return GetXMLFromTimetable(timetable);
            }
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        }

        private XDocument GetXMLFromTimetable(List<TimetableItem> timetable)
        {
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootTimetable = new XElement("timetable");

            XElement monday = new XElement("monday");
            XElement tuesday = new XElement("tuesday");
            XElement wednesday = new XElement("wednesday");
            XElement thursday = new XElement("thursday");
            XElement friday = new XElement("friday");
            XElement saturday = new XElement("saturday");
            XElement sunday = new XElement("sunday");

            foreach (TimetableItem item in timetable)
            {
                XElement timetableSubitem = new XElement("tableItem");
                timetableSubitem.Add(new XElement("startTime", item.StartTime.ToString("HH:mm")));
                timetableSubitem.Add(new XElement("endTime", item.EndTime.ToString("HH:mm")));
                timetableSubitem.Add(new XElement("className", item.ClassName));
                timetableSubitem.Add(new XElement("classType", (int)item.TypeOfClass));
                timetableSubitem.Add(new XElement("teacherName", item.TeacherName));
                timetableSubitem.Add(new XElement("roomNumber", item.RoomNumber));
                timetableSubitem.Add(new XElement("frequency", (int)item.Frequency));
                timetableSubitem.Add(new XElement("optionalPackage", item.OptionalPackage));

                XElement group = new XElement("group");
                group.Add(new XElement("yearOfStudy", (int)item.StudyGroup.YearOfStudy));
                group.Add(new XElement("halfYearOfStudy", item.StudyGroup.HalfYearOfStudy));
                group.Add(new XElement("groupNumber", item.StudyGroup.Number));

                timetableSubitem.Add(group);

                switch (item.Day)
                {
                    case DayOfWeek.Friday:
                        friday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Monday:
                        monday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Saturday:
                        saturday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Sunday:
                        sunday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Thursday:
                        thursday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Tuesday:
                        tuesday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Wednesday:
                        wednesday.Add(timetableSubitem);
                        break;
                    default:
                        break;
                }
            }

            rootTimetable.Add(monday);
            rootTimetable.Add(tuesday);
            rootTimetable.Add(wednesday);
            rootTimetable.Add(thursday);
            rootTimetable.Add(friday);
            rootTimetable.Add(saturday);
            rootTimetable.Add(sunday);
            document.Add(rootTimetable);
            return document;
        }
    }
}

