using System;
using System.Collections.Generic;

namespace Objects
{
    public class Subject : MongoDBObject
    {
        public List<string> TeacherList { get; set; }
        public string Name { get; set; }
        public List<string> Websites { get; set; }
        public bool IsOptional { get; set; }

        public Subject()
        {
            TeacherList = new List<string>();
            Name = String.Empty;
            Websites = new List<string>();
        }
    }
}