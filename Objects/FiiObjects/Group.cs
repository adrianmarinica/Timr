using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Group
    {
        public string Number { get; set; }
        public HalfYear HalfYearOfStudy { get; set; }
        public StudyYear YearOfStudy { get; set; }

        public Group()
        {
            Number = String.Empty;
        }

        public bool IsContainedIn(Group feedGroup)
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
