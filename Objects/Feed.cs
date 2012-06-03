using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Feed : MongoDBObject
    {
        public User Sender { get; set; }
        public List<Group> ReceiverGroups { get; set; }
        public List<User> ReceiverUsers { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }

        public Feed()
        {
            ReceiverGroups = new List<Group>();
            ReceiverUsers = new List<User>();
            Message = String.Empty;
        }
    }
}
