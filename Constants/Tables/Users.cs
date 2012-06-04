using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Constants.Tables
{
    public sealed class Users
    {
        public sealed class Username
        {
            public const string Key = "Id";
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
            public const string Key = "UserType";
            public const BsonType Type = BsonType.Int32;
        }
        public sealed class LastLoginDate
        {
            public const string Key = "LastLoginDate";
            public const BsonType Type = BsonType.DateTime;
        }
        public sealed class LastActivityDate
        {
            public const string Key = "LastActivityDate";
            public const BsonType Type = BsonType.DateTime;
        }
        public sealed class PasswordQuestion
        {
            public const string Key = "PasswordQuestion";
            public const BsonType Type = BsonType.String;
        }
        public sealed class PasswordAnswer
        {
            public const string Key = "PasswordAnswer";
            public const BsonType Type = BsonType.String;
        }
        public sealed class LastPasswordChangedDate
        {
            public const string Key = "SubscribedWebsites";
            public const BsonType Type = BsonType.DateTime;
        }
        public sealed class CreationDate
        {
            public const string Key = "CreationDate";
            public const BsonType Type = BsonType.DateTime;
        }
        public sealed class IsLockedOut
        {
            public const string Key = "IsLockedOut";
            public const BsonType Type = BsonType.Boolean;
        }
        public sealed class LastLockoutDate
        {
            public const string Key = "LastLockoutDate";
            public const BsonType Type = BsonType.DateTime;
        }
    }
}
