using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class TimetableNotification : Notification
    {
        public string ModifiedGroup { get; set; }
        public DayOfWeek Day { get; set; }
        public TimetableItem ModifiedItem { get; set; }
        public TimetableNotificationTypes TimetableNotificationType { get; set; }
    }
}
