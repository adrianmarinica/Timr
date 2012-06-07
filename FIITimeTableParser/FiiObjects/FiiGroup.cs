using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIITimetableParser.FiiObjects
{
    public class FiiGroup
    {
        public string Number { get; set; }
        public HalfYear HalfYearOfStudy { get; set; }
        public StudyYear YearOfStudy { get; set; }

        public FiiGroup()
        {
            Number = String.Empty;
        }

        public bool IsContainedIn(FiiGroup feedGroup)
        {
            bool contained = false;
            if (!String.IsNullOrEmpty(feedGroup.Number)) // it is meant for specific group
            {
                if (this.Number == feedGroup.Number)
                {
                    if (this.HalfYearOfStudy == feedGroup.HalfYearOfStudy &&
                        this.YearOfStudy == feedGroup.YearOfStudy)
                        contained = true;
                }
            }
            else if (feedGroup.HalfYearOfStudy != HalfYear.None)
            {
                if (this.HalfYearOfStudy == feedGroup.HalfYearOfStudy)
                {
                    if (this.YearOfStudy == feedGroup.YearOfStudy)
                        contained = true;
                }
            }
            else if (feedGroup.YearOfStudy != StudyYear.None)
            {
                if (this.YearOfStudy == feedGroup.YearOfStudy)
                {
                    contained = true;
                }
            }
            return contained;
        }
    }
}
