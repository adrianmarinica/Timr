using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class TeacherRequest : MongoDBObject
    {
        public string TeacherUsername { get; set; }
        public string FacultyUsername { get; set; }
        public DateTime DateSent { get; set; }
    }
}
