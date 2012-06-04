using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer;
using DataAccessLayer.Collections;
using WebsiteMonitor;

namespace BusinessLogic
{
    public class MonitoredWebsitesBL
    {
        public MonitoredWebsitesDAL dal
        {
            get
            {
                return new MonitoredWebsitesDAL();
            }
        }

        public List<MonitoredWebsite> GetAllSubscribedWebsites(string userName)
        {
            return dal.GetAllMonitoredWebsites(userName);
        }

        public List<MonitoredWebsite> GetAllModifiedWebsites(string userName)
        {
            List<MonitoredWebsite> list = dal.GetMonitoredWebsitesByUsername(userName);
            if(list != null)
            {
                Monitor monitor = new Monitor();
                list = monitor.FilterAllModifiedWebsites(list);
                dal.SaveMonitoredWebsites(list);
            }
            return list;
        }

        public void SaveMonitoredWebsite(MonitoredWebsite website)
        {
            website.HashedContent = Monitor.GetMD5Hash(website.Id);
            dal.SaveMonitoredWebsite(website);
        }

        public MonitoredWebsite GetWebsite(string website)
        {
            return dal.GetWebsite(website);
        }
    }
}
