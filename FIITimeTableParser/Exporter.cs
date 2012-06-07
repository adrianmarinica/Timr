using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FIITimetableParser.FiiObjects;
using Objects;

namespace FIITimetableParser
{
    public class Exporter
    {
        public string ConvertToXML(List<FiiTimetableItem> timetable, List<Subject> availableSubjects)
        {
            if (timetable != null)
            {
                return GetXMLFromTimetable(timetable, availableSubjects).ToString();
            }
            return String.Empty;
        }

        public XDocument ConvertToXDocument(List<FiiTimetableItem> timetable, List<Subject> availableSubjects)
        {
            if (timetable != null)
            {
                return GetXMLFromTimetable(timetable, availableSubjects);
            }
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
        }

        private XDocument GetXMLFromTimetable(List<FiiTimetableItem> timetable, List<Subject> availableSubjects)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var rootTimetable = new XElement("timetable");

            var monday = new XElement("monday");
            var tuesday = new XElement("tuesday");
            var wednesday = new XElement("wednesday");
            var thursday = new XElement("thursday");
            var friday = new XElement("friday");
            var saturday = new XElement("saturday");
            var sunday = new XElement("sunday");

            foreach (var item in timetable)
            {
                var timetableSubitem = new XElement("tableItem");
                if (availableSubjects != null)
                {
                    var subjectItem =
                        availableSubjects.Find(delegate(Subject subject) { return subject.Name == item.ClassName; });
                    if (subjectItem != null)
                        timetableSubitem.Add(new XAttribute("id", subjectItem._id.ToString()));
                }
                timetableSubitem.Add(new XElement("startTime", item.StartTime));
                timetableSubitem.Add(new XElement("endTime", item.EndTime));
                timetableSubitem.Add(new XElement("className", item.ClassName));
                timetableSubitem.Add(new XElement("classType", (int)item.TypeOfClass));
                timetableSubitem.Add(new XElement("teacherName", item.TeacherName));
                timetableSubitem.Add(new XElement("roomNumber", item.RoomNumber));
                timetableSubitem.Add(new XElement("frequency", (int)item.Frequency));
                timetableSubitem.Add(new XElement("optionalPackage", item.OptionalPackage));

                var groups = new XElement("groups");

                foreach (var groupItem in item.StudyGroups)
                {
                    var group = new XElement("group");
                    group.Add(new XElement("yearOfStudy", (int)groupItem.YearOfStudy));
                    group.Add(new XElement("halfYearOfStudy", groupItem.HalfYearOfStudy));
                    group.Add(new XElement("groupNumber", groupItem.Number));
                    groups.Add(group);
                }

                timetableSubitem.Add(groups);

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

