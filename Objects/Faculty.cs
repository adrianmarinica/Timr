using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Faculty : User
    {
        public List<string> Teachers { get; set; }
        public List<string> Students { get; set; }
        public GenericGroup Groups { get; set; }

        public Faculty()
        {
            Teachers = new List<string>();
            UserType = UserTypes.Faculty;
            Students = new List<string>();
        }
    }
}
