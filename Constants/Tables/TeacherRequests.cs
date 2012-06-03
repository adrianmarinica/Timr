using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Constants.Tables
{
    public class TeacherRequestsBaseTable : BaseTable
    {
        public sealed class TeacherUsername
        {
            public const string Key = "TeacherUsername";
            public const BsonType Type = BsonType.String;
        }
        public sealed class FacultyUsername
        {
            public const string Key = "FacultyUsername";
            public const BsonType Type = BsonType.String;
        }
        public sealed class DateSent
        {
            public const string Key = "DateSent";
            public const BsonType Type = BsonType.DateTime;
        }
    }
}
