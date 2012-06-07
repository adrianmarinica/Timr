using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using FIITimetableParser.FiiObjects;
using Logger;
using HtmlAgilityPack;
using Objects;

namespace FIITimetableParser
{
    public class Parser
    {
        public Parser()
        {
            
        }

        #region PublicMethods

        public List<FiiTimetableItem> GetTimetableForYear(StudyYear year, HalfYear halfYear = HalfYear.None)
        {
            List<FiiTimetableItem> timetable;

            string tempYear = Enum.GetName(typeof(StudyYear), year);
            string tempHalfYear = Enum.GetName(typeof(HalfYear), halfYear);
            if (tempHalfYear == "None") tempHalfYear = String.Empty;

            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(String.Format("http://thor.info.uaic.ro/~orar/participanti/orar_{0}{1}.html", tempYear, tempHalfYear));
                doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("\r\n", "");

                timetable = ParseTable(doc, TimetableType.Year);
            }
            catch (WebException ex)
            {
                ExceptionLogger.Log(ex);
                timetable = null;
            }
            catch (NotSupportedException ex)
            {
                ExceptionLogger.Log(ex);
                timetable = null;
            }
            return timetable;
        }

        public List<FiiTimetableItem> GetTimetableForGroup(StudyYear year, HalfYear halfYear, string groupNumber)
        {
            List<FiiTimetableItem> timetable;
            string tempYear = Enum.GetName(typeof(StudyYear), year);
            string tempHalfYear = Enum.GetName(typeof(HalfYear), halfYear);
            if (tempHalfYear == "None")
                tempHalfYear = String.Empty;

            try
            {
                var hw = new HtmlWeb();
                var doc = hw.Load(String.Format("http://thor.info.uaic.ro/~orar/participanti/orar_{0}{1}{2}.html", tempYear, tempHalfYear, groupNumber));
                doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("\r\n", "");
                timetable = ParseTable(doc, TimetableType.Group, year, halfYear, groupNumber);
            }
            catch (WebException ex)
            {
                ExceptionLogger.Log(ex);
                timetable = null;
            }
            catch (NotSupportedException ex)
            {
                ExceptionLogger.Log(ex);
                timetable = null;
            }
            return timetable;
        }

        #endregion

        #region PrivateMethods

        private List<FiiTimetableItem> ParseTable(HtmlDocument document, TimetableType type, StudyYear studyYear = StudyYear.None, HalfYear halfYear = HalfYear.None, string groupNumber = "")
        {
            var timetable = new List<FiiTimetableItem>();
            if (document.DocumentNode.Descendants("table").Any())
            {
                foreach (HtmlNode table in document.DocumentNode.Descendants("table"))
                {
                    DayOfWeek day = DayOfWeek.Monday;
                    foreach (HtmlNode tableRow in table.Descendants("tr"))
                    {
                        bool isDirty = false;
                        if (tableRow.Attributes.Count == 0)
                        {
                            FiiTimetableItem item = null;
                            int index = 0;
                            foreach (HtmlNode tableCell in tableRow.Descendants("td"))
                            {
                                string innerText = tableCell.InnerText.Trim();
                                if (tableCell.Attributes.Count > 0 && tableCell.Attributes[0].Value == "10")
                                {
                                    day = new DayOfWeek();
                                    day = GetDayFromCell(innerText);
                                }
                                else
                                {
                                    switch (index)
                                    {
                                        case 0:
                                            item = new FiiTimetableItem();
                                            item.Day = day;
                                            int startHours;
                                            int startMinutes;
                                            if (Int32.TryParse(innerText.Substring(0, 2), out startHours) &&
                                                Int32.TryParse(innerText.Substring(3, 2), out startMinutes))
                                            {
                                                item.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startHours, startMinutes, 0).ToString("HH:mm");
                                            }
                                            break;
                                        case 1:
                                            int endHours;
                                            int endMinutes;
                                            if (Int32.TryParse(innerText.Substring(0, 2), out endHours) &&
                                                Int32.TryParse(innerText.Substring(3, 2), out endMinutes))
                                            {
                                                item.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endHours, endMinutes, 0).ToString("HH:mm");
                                            }
                                            break;
                                        case 2:
                                            if (type == TimetableType.Year)
                                            {
                                                item.StudyGroups = GetGroupsFromCell(innerText);
                                                index--;
                                                type = TimetableType.Group;
                                                isDirty = true;
                                            }
                                            else
                                            {
                                                item.ClassName = innerText;
                                                if (item.StudyGroup == null)
                                                {
                                                    item.StudyGroups = new List<FiiGroup>();
                                                    item.StudyGroups.Add(new FiiGroup
                                                    {
                                                        YearOfStudy = studyYear,
                                                        HalfYearOfStudy = halfYear,
                                                        Number = groupNumber
                                                    });
                                                }
                                                if (isDirty)
                                                    type = TimetableType.Year;
                                            }
                                            break;
                                        case 3:
                                            item.TypeOfClass = GetClassTypeFromCell(innerText);
                                            break;
                                        case 4:
                                            item.TeacherName = innerText;
                                            break;
                                        case 5:
                                            item.RoomNumber = innerText;
                                            break;
                                        case 6:
                                            item.Frequency = GetFrequencyFromCell(innerText);
                                            break;
                                        case 7:
                                            item.OptionalPackage = GetOptionalPackageFromCell(innerText);
                                            break;
                                    }
                                    index++;
                                }
                            }
                            if (item != null)
                                timetable.Add(item);
                        }
                    }
                }
            }
            return timetable;
        }

        public FiiGroup GetGroupFromCell(string text)
        {
            FiiGroup group = new FiiGroup();
            int yearIndex = text.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            string year = text.Substring(0, yearIndex + 1);

            if (Enum.IsDefined(typeof(StudyYear), year))
                group.YearOfStudy = (StudyYear)Enum.Parse(typeof(StudyYear), year);

            if (text.Length >= yearIndex + 2)
            {
                string halfYear = text.Substring(yearIndex + 1, 1);
                if (Enum.IsDefined(typeof(HalfYear), halfYear))
                    group.HalfYearOfStudy = (HalfYear)Enum.Parse(typeof(HalfYear), halfYear);

                if (text.Length >= yearIndex + 3)
                {
                    group.Number = text.Substring(yearIndex + 2);
                }
            }
            return group;
        }

        private List<FiiGroup> GetGroupsFromCell(string text)
        {
            List<FiiGroup> groups = new List<FiiGroup>();
            string[] texts = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < texts.Length ; i++)
            {
                string item = texts[i];
                item = item.Trim();
                FiiGroup group = new FiiGroup();
                int yearIndex = item.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                string year = item.Substring(0, yearIndex + 1);

                if (Enum.IsDefined(typeof(StudyYear), year))
                    group.YearOfStudy = (StudyYear)Enum.Parse(typeof(StudyYear), year);

                if (item.Length >= yearIndex + 2)
                {
                    string halfYear = item.Substring(yearIndex + 1, 1);
                    if (Enum.IsDefined(typeof(HalfYear), halfYear))
                        group.HalfYearOfStudy = (HalfYear)Enum.Parse(typeof(HalfYear), halfYear);

                    if (item.Length >= yearIndex + 3)
                    {
                        group.Number = item.Substring(yearIndex + 2);
                    }
                }
                groups.Add(group);
            }
            return groups;
        }

        private int GetOptionalPackageFromCell(string text)
        {
            int value;
            if (Int32.TryParse(text, out value))
            {
                return value;
            }
            else return -1;
        }

        private ClassFrequency GetFrequencyFromCell(string text)
        {
            switch (text)
            {
                case "Pare":
                    return ClassFrequency.EvenWeeks;
                case "Impare":
                    return ClassFrequency.OddWeeks;
                default:
                    return ClassFrequency.EveryWeek;
            }
        }

        private ClassType GetClassTypeFromCell(string text)
        {
            switch (text)
            {
                case "Curs":
                    return ClassType.Course;
                case "Seminar":
                    return ClassType.Seminar;
                case "Laborator":
                    return ClassType.Laboratory;
                default:
                    return ClassType.Course;
            }
        }

        private DayOfWeek GetDayFromCell(string text)
        {
            if (text.Contains("Luni"))
                return DayOfWeek.Monday;
            else if (text.Contains("Marti"))
                return DayOfWeek.Tuesday;
            else if (text.Contains("Miercuri"))
                return DayOfWeek.Wednesday;
            else if (text.Contains("Joi"))
                return DayOfWeek.Thursday;
            else if (text.Contains("Vineri"))
                return DayOfWeek.Friday;
            else if (text.Contains("Sambata"))
                return DayOfWeek.Saturday;
            else
                return DayOfWeek.Sunday;
        }

        public List<FiiTimetableItem> GetFullTimetable()
        {
            var parser = new Parser();
            var list = new List<FiiTimetableItem>();
            parser.GetTimetableForGroup(StudyYear.I1, HalfYear.A, "1");
            list.AddRange(parser.GetTimetableForYear(StudyYear.I1, HalfYear.A));
            list.AddRange(parser.GetTimetableForYear(StudyYear.I1, HalfYear.B));
            list.AddRange(parser.GetTimetableForYear(StudyYear.I2, HalfYear.A));
            list.AddRange(parser.GetTimetableForYear(StudyYear.I2, HalfYear.B));
            list.AddRange(parser.GetTimetableForYear(StudyYear.I3, HalfYear.A));
            list.AddRange(parser.GetTimetableForYear(StudyYear.I3, HalfYear.B));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MIS1));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MIS2));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MLC1));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MLC2));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MOC1));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MOC2));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MSD1));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MSD2));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MSI1));
            list.AddRange(parser.GetTimetableForYear(StudyYear.MSI2));
            return list;
        }

        #endregion
    }
}
