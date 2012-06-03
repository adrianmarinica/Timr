using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class MonitoredWebsiteNotification : Notification
    {
        public string WebsiteId { get; set; }

        public MonitoredWebsiteNotification()
        {
            WebsiteId = String.Empty;
        }
    }
}
