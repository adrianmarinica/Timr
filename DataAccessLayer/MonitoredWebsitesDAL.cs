using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;
using Constants.Tables;

namespace DataAccessLayer
{
    public class MonitoredWebsitesDAL
    {
        public List<MonitoredWebsite> GetAllMonitoredWebsites()
        {
            return MonitoredWebsitesCollection.Collection.FindAllAs<MonitoredWebsite>().ToList();
        }

        public List<MonitoredWebsite> GetMonitoredWebsitesByUsername(string userName)
        {
            UsersDAL uDAL = new UsersDAL();
            List<string> siteLinks = uDAL.GetSubscribedWebsiteLinks(userName);
            List<MonitoredWebsite> siteList = new List<MonitoredWebsite>();
            foreach (var item in siteLinks)
            {
                MonitoredWebsite temp = MonitoredWebsitesCollection.Collection.FindOneByIdAs<MonitoredWebsite>(item);
                if (temp != null)
                {
                    siteList.Add(temp);
                }
            }
            return siteList;
        }

        public void SaveMonitoredWebsites(List<MonitoredWebsite> list)
        {
            foreach (var item in list)
            {
                MonitoredWebsitesCollection.Collection.Save<MonitoredWebsite>(item);
            }
        }

        public void SaveMonitoredWebsite(MonitoredWebsite website)
        {
            MonitoredWebsitesCollection.Collection.Save(website);
        }

        public List<MonitoredWebsite> GetAllMonitoredWebsites(string userName)
        {
            List<MonitoredWebsite> list = new List<MonitoredWebsite>();
            var user = UsersCollection.Collection.FindOneByIdAs<User>(userName);
            if(user.UserType == UserTypes.Student)
            {
                foreach(var item in (user as Student).SubscribedWebsites)
                {
                    MonitoredWebsite website = MonitoredWebsitesCollection.Collection.FindOneByIdAs<MonitoredWebsite>(item);
                    if (website != null)
                        list.Add(website);
                }
            }
            return list;
        }

        public MonitoredWebsite GetWebsite(string website)
        {
            return MonitoredWebsitesCollection.Collection.FindOneByIdAs<MonitoredWebsite>(website);
        }
    }
}
