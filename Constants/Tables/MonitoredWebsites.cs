using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Constants.Tables
{
    public class MonitoredWebsites : BaseTable
    {
        public sealed class OwnerName
        {
            public const string Key = "Owner.Name";
            public const BsonType Type = BsonType.String;
        }
        public sealed class HashedContent
        {
            public const string Key = "HashedContent";
            public const BsonType Type = BsonType.String;
        }
    }
}
