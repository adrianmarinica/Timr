using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public abstract class Notification : MongoDBObject
    {
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; }
    }
}
