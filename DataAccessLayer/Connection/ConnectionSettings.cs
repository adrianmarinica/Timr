using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DataAccessLayer
{
    public static class ConnectionSettings
    {
        public static string ConnectionString 
        { 
            get
            {
                return "mongodb://localhost/?safe=true";
                // return ConfigurationManager.ConnectionStrings["MongoDBServer"].ConnectionString;
            }
        }
    }
}
