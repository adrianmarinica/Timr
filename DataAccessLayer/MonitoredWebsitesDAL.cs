using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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
            var notificationsDAL = new NotificationsDAL();

            var notification = new MonitoredWebsiteNotification
                                                            {
                                                                NotificationType =
                                                                    NotificationTypes.MonitoredWebsitesNotification,
                                                                WebsiteId = website.Id,
                                                                SentDate = DateTime.Now,
                                                                Title =
                                                                    String.Format(
                                                                        "Website at {0} has changed its content.",
                                                                        website.Id)
                                                            };

            notificationsDAL.InsertNotification(notification);

            var usersDAL = new UsersDAL();
            var allStudents = usersDAL.GetAllStudents();

            if (allStudents != null)
                foreach (var student in allStudents)
                {
                    if (student.SubscribedWebsites.Find(delegate(string gr)
                                                            {
                                                                return gr == website.Id;
                                                            }

                    ).Any())
                    {
                        student.Notifications.Add(new UserNotification
                        {
                            NotificationId = notification._id.ToString(),
                            NotificationType = NotificationTypes.MonitoredWebsitesNotification,
                            UserSolved = false
                        });
                    }
                    usersDAL.SaveStudent(student);
                }
        }

        public List<MonitoredWebsite> GetAllMonitoredWebsites(string userName)
        {
            List<MonitoredWebsite> list = new List<MonitoredWebsite>();
            var usersDAL = new UsersDAL();
            var user = usersDAL.GetStudent(userName);
            
            if(user != null)
            {
                foreach(var item in user.SubscribedWebsites)
                {
                    MonitoredWebsite website = MonitoredWebsitesCollection.Collection.FindOneByIdAs<MonitoredWebsite>(item);
                    if (website != null)
                        list.Add(website);
                }
            }
            return list;
        }

        public string GetAllSubscribedWebsitesAsXml(string username)
        {
            var list = GetAllMonitoredWebsites(username);
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var websites = new XElement("websites");
            foreach (var monitoredWebsite in list)
            {
                var website = new XElement("website");
                
                website.Add(new XElement("owner", monitoredWebsite.Owner));
                website.Add(new XElement("lastModified", monitoredWebsite.LastModified.ToString(CultureInfo.InvariantCulture)));
                website.Add(new XElement("title", monitoredWebsite.Title));
                website.Add(new XElement("subjectId", monitoredWebsite.WebsiteSubjectId));
                website.Add(new XElement("link", monitoredWebsite.Id));
                websites.Add(website);
            }
            document.Add(websites);
            return document.ToString();
        }

        public MonitoredWebsite GetWebsite(string website)
        {
            return MonitoredWebsitesCollection.Collection.FindOneByIdAs<MonitoredWebsite>(website);
        }

        public void InsertWebsite(MonitoredWebsite website)
        {
            MonitoredWebsitesCollection.Collection.Insert(website);
        }

        public List<MonitoredWebsite> GetAllWebsites()
        {
            return MonitoredWebsitesCollection.Collection.FindAllAs<MonitoredWebsite>().ToList();
        }
    }
}
