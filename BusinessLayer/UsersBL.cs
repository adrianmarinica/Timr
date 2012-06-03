using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using Objects;
using DataAccessLayer;

namespace BusinessLogic
{
    public class UsersBL
    {
        private UsersDAL dal
        {
            get
            {
                return new UsersDAL();
            }
        }
        public bool InsertUser(string username, string password, string email, UserTypes userType)
        {
            return dal.InsertUser(new User
            {
                Id = username,
                Password = password,
                Email = email,
                UserType = userType
            });
        }

        public bool ValidateUser(string username, string password)
        {
            return dal.ValidateUser(username, password);
        }

        public void DeleteUser(string username, string password)
        {
            dal.DeleteUser(username, password);
        }

        public void AddSubscribedWebsite(string userName, string websiteLink, string websiteOwner)
        {
            if (!String.IsNullOrEmpty(websiteLink))
            {
                dal.AddSubscribedWebsite(userName, ParseLink(websiteLink));
                MonitoredWebsitesBL bl = new MonitoredWebsitesBL();
                MonitoredWebsite website = new MonitoredWebsite
                {
                    Id = websiteLink,
                    Title = websiteOwner
                };
                bl.SaveMonitoredWebsite(website);
            }
        }

        private string ParseLink(string rawLink)
        {
            if (!rawLink.StartsWith("http://"))
            {
                rawLink = String.Format("http://{0}", rawLink);
            }
            if (!rawLink.EndsWith("/"))
            {
                rawLink = String.Format("{0}/", rawLink);
            }
            return rawLink;
        }

        public User GetUser(string username, string password)
        {
            return dal.GetUser(username, password);
        }

        public User GetUser(string username)
        {
            return dal.GetUser(username);
        }

        public List<Teacher> GetTeachersByFaculty(string facultyUserName)
        {
            return dal.GetTeachersByFaculty(facultyUserName);
        }
    }
}