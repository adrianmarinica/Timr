using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Teacher : User
    {
        public List<UserNotification> Notifications { get; set; }
        public List<string> SubmittedFeeds { get; set; }
        public Teacher()
        {
            Notifications = new List<UserNotification>();
            SubmittedFeeds = new List<string>();
            UserType = UserTypes.Teacher;
        }
    }
}
