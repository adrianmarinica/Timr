using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class User : MongoDBObject
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public UserTypes UserType { get; set; }

        public User()
        {
            Password = String.Empty;
            Email = String.Empty;
        }
    }
}
