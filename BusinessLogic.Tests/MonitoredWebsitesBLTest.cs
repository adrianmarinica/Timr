using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Objects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class MonitoredWebsitesBLTest
    {
        [Test]
        public void ShouldInsertWebsite()
        {
            var monitoredWebsitesBL = new MonitoredWebsitesBL();
            var website = new MonitoredWebsite
                              {
                                  Title = "Tehnologii Web",
                                  Owner = "busaco",
                                  WebsiteSubjectId = "2",
                                  Id = "http://info.uaic.ro/~busaco/"
                              };
            monitoredWebsitesBL.SaveMonitoredWebsite(website);

            website = new MonitoredWebsite
            {
                Title = "Arhitectura Calculatoarelor",
                Owner = "rvlad",
                WebsiteSubjectId = "3",
                Id = "http://info.uaic.ro/~rvlad/"
            };

            monitoredWebsitesBL.SaveMonitoredWebsite(website);
        }
    }
}
