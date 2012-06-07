using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using FIITimetableParser;
using DataAccessLayer;
using FIITimetableParser.FiiObjects;

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

        public List<FiiTimetableItem> GetTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            return _parser.GetTimetableForYear(year, halfYear);
        }

        public List<FiiTimetableItem> GetTimetableForMastersYear(StudyYear year)
        {
            return _parser.GetTimetableForYear(year);
        }

        public string GetXMLTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            var subjectsBL = new SubjectsBL();
            return _exporter.ConvertToXML(_parser.GetTimetableForYear(year, halfYear), subjectsBL.GetAllSubjects());
        }

        public string GetXMLTimetableForMastersYear(StudyYear year)
        {
            var subjectsBL = new SubjectsBL();
            return _exporter.ConvertToXML(_parser.GetTimetableForYear(year), subjectsBL.GetAllSubjects());
        }
    }
}
