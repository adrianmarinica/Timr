using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIITimetableParser;
using Objects;

namespace DataAccessLayer
{
    public class FiiTimetableDAL
    {
        public void RefreshTimetable()
        {
            List<TimetableItem> list = GetFullTimetable();
            //TODO ceva

        }

        public List<TimetableItem> GetFullTimetable()
        {
            Parser parser = new Parser();
            List<TimetableItem> list = new List<TimetableItem>();
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
    }
}
