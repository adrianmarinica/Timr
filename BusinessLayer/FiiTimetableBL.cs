using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using FIITimetableParser;
using DataAccessLayer;

namespace BusinessLogic
{
    public class FiiTimetableBL
    {
        public Parser _parser
        {
            get
            {
                return new Parser();
            }
        }

        public Exporter _exporter
        {
            get
            {
                return new Exporter();
            }
        }

        private FiiTimetableDAL dal
        {
            get
            {
                return new FiiTimetableDAL();
            }
        }

        public List<TimetableItem> GetTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            return _parser.GetTimetableForYear(year, halfYear);
        }

        public List<TimetableItem> GetTimetableForMastersYear(StudyYear year)
        {
            return _parser.GetTimetableForYear(year);
        }

        public string GetXMLTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            return _exporter.ConvertToXML(_parser.GetTimetableForYear(year, halfYear));
        }

        public string GetXMLTimetableForMastersYear(StudyYear year)
        {
            return _exporter.ConvertToXML(_parser.GetTimetableForYear(year));
        }

        public void RefreshTimetable()
        {
            dal.RefreshTimetable();
        }
    }
}
