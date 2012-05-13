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
    }
}
