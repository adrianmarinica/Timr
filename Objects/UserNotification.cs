using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class UserNotification : MongoDBObject
    {
        public string NotificationId { get; set; }
        public bool UserSolved { get; set; }

        public UserNotification()
        {
            NotificationId = String.Empty;
        }
    }
}
