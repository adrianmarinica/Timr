using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Faculty : User
    {
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public GenericGroup Groups { get; set; }

        public Faculty()
        {
            Teachers = new List<Teacher>();
            UserType = UserTypes.Faculty;
            Students = new List<Student>();
            Groups = new GenericGroup();
        }
    }
}
