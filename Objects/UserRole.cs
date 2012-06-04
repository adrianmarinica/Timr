using System;

namespace Objects
{
    public class UserRole : MongoDBObject
    {
        public UserTypes UserType { get; set; }
        public string Name { get; set; }

        public UserRole()
        {
            Name = String.Empty;
        }
    }
}