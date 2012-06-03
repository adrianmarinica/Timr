using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class FeedNotification : Notification
    {
        public string FeedId { get; set; }

        public FeedNotification()
        {
            FeedId = String.Empty;
        }
    }
}
