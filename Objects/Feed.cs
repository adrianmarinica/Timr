using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Feed : MongoDBObject
    {
        public string Sender { get; set; }
        public List<string> ReceiverGroups { get; set; }
        public List<string> ReceiverUsers { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }
        public string Faculty { get; set; }
        public Feed()
        {
            ReceiverGroups = new List<string>();
            ReceiverUsers = new List<string>();
            Message = String.Empty;
            Faculty = String.Empty;
        }
    }
}
