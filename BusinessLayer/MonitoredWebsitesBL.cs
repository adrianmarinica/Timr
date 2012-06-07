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

        public string GetAllSubscribedWebsitesAsXml(string username)
        {
            return dal.GetAllSubscribedWebsitesAsXml(username);
        }

        public List<MonitoredWebsite> GetAllModifiedWebsites(string userName)
        {
            List<MonitoredWebsite> list = dal.GetMonitoredWebsitesByUsername(userName);
            if(list != null)
            {
                list = Monitor.FilterAllModifiedWebsites(list);
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

        public void InsertWebsite(MonitoredWebsite website)
        {
            dal.InsertWebsite(website);
        }

        public List<MonitoredWebsite> GetAllWebsites()
        {
            return dal.GetAllWebsites();
        }

        public void SaveMonitoredWebsites(List<MonitoredWebsite> filterAllModifiedWebsites)
        {
            dal.SaveMonitoredWebsites(filterAllModifiedWebsites);
        }
    }
}
