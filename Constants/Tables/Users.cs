using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Constants.Tables
{
    public sealed class Users : BaseTable
    {
        public sealed class Username
        {
            public const string Key = "Username";
            public const BsonType Type = BsonType.String;
        }
        public sealed class Password
        {
            public const string Key = "Password";
            public const BsonType Type = BsonType.String;
        }
        public sealed class Email
        {
            public const string Key = "Email";
            public const BsonType Type = BsonType.String;
        }
        public sealed class UserType
        {
            public const string Key = "Type";
            public const BsonType Type = BsonType.Int32;
        }
    }
}
