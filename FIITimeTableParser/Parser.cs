using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Objects;
using System.Net;
using Logger;
using HtmlAgilityPack;

namespace FIITimetableParser
{
    public class Parser
    {
        public Parser()
        {

        }

        #region PublicMethods

        public List<TimetableItem> GetFullTimetable()
        {
            List<TimetableItem> timetable = new List<TimetableItem>();
            return timetable;
        }

        public List<TimetableItem> GetTimetable(StudyYear year, string groupName)
        {
            List<TimetableItem> timetable;

            using (WebClient client = new WebClient())
            {
                string tempYear = Enum.GetName(typeof(StudyYear), year);
                try
                {
                    HtmlWeb hw = new HtmlWeb();
                    HtmlDocument doc = hw.Load(String.Format("http://thor.info.uaic.ro/~orar/participanti/orar_{0}{1}.html", tempYear, groupName));
                    doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("\r\n", "");

                    timetable = ParseTable(doc);
                }
                catch (WebException ex)
                {
                    Logger.ExceptionLogger.Log(ex);
                    timetable = null;
                }
                catch (NotSupportedException ex)
                {
                    Logger.ExceptionLogger.Log(ex);
                    timetable = null;
                }
            }
            return timetable;
        }

        #endregion

        #region PrivateMethods

        private List<TimetableItem> ParseTable(HtmlDocument document)
        {

            List<TimetableItem> timetable = new List<TimetableItem>();



            //            if(document.DocumentNode.ChildNodes.Count >= 2)
            //            {
            //                HtmlNode html = document.DocumentNode.ChildNodes[1];
            //                if(html.ChildNodes
            //            }
            if (document.DocumentNode.Descendants("table").Count() > 0)
            {
                DayOfWeek day = DayOfWeek.Monday;
                foreach (HtmlNode tableRow in document.DocumentNode.Descendants("table").ElementAt(0).Descendants("tr"))
                {
                    if (tableRow.Attributes.Count == 0)
                    {
                        TimetableItem item = null;
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
                                        item = new TimetableItem();
                                        item.Day = day;
                                        int startHours;
                                        int startMinutes;
                                        if (Int32.TryParse(innerText.Substring(0, 2), out startHours) &&
                                            Int32.TryParse(innerText.Substring(3, 2), out startMinutes))
                                        {
                                            item.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startHours, startMinutes, 0);
                                        }
                                        break;
                                    case 1:
                                        int endHours;
                                        int endMinutes;
                                        if (Int32.TryParse(innerText.Substring(0, 2), out endHours) &&
                                            Int32.TryParse(innerText.Substring(3, 2), out endMinutes))
                                        {
                                            item.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endHours, endMinutes, 0);
                                        }
                                        break;
                                    case 2:
                                        item.ClassName = innerText;
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
            return timetable;
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

        #endregion

    }
}
