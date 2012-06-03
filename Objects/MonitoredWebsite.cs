using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class MonitoredWebsite
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HashedContent { get; set; }

        public MonitoredWebsite()
        {
            Id = String.Empty;
            Title = String.Empty;
            HashedContent = String.Empty;
        }
    }
}
