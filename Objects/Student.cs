using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Student : User
    {
        public List<string> SubscribedWebsites { get; set; }
        public List<Group> SubscribedGroups { get; set; }
        public List<UserNotification> Notifications { get; set; }
        public List<string> SubscribedFaculties { get; set; }

        public Student()
        {
            SubscribedWebsites = new List<string>();
            SubscribedGroups = new List<Group>();
            SubscribedFaculties = new List<string>();
            Notifications = new List<UserNotification>();
            UserType = UserTypes.Student;
        }
    }
}
