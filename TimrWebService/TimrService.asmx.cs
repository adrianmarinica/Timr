using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Objects;
using BusinessLogic;

namespace TimrWebService
{
    /// <summary>
    /// Summary description for TimrService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TimrService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            FiiTimetableBL bl = new FiiTimetableBL();
            return bl.GetXMLTimetableForBachelorYear(year, halfYear);
        }

        [WebMethod]
        public string GetTimetableForMastersYear(StudyYear year)
        {
            FiiTimetableBL bl = new FiiTimetableBL();
            return bl.GetXMLTimetableForMastersYear((StudyYear)year);
        }

        [WebMethod]
        public bool InsertUser(string username, string password, string email, UserTypes userType)
        {
            UsersBL bl = new UsersBL();
            return bl.InsertUser(username, password, email, userType);
        }

        [WebMethod]
        public bool ValidateUser(string username, string password)
        {
            UsersBL bl = new UsersBL();
            return bl.ValidateUser(username, password);
        }

        [WebMethod]
        public void DeleteUser(string username, string password)
        {
            UsersBL bl = new UsersBL();
            bl.DeleteUser(username, password);
        }

        [WebMethod]
        public List<MonitoredWebsite> GetAllMonitoredWebsites(string username)
        {
            MonitoredWebsitesBL bl = new MonitoredWebsitesBL();
            return bl.GetAllSubscribedWebsites(username);
        }

        [WebMethod]
        public List<MonitoredWebsite> GetAllModifiedWebsites(string username)
        {
            MonitoredWebsitesBL bl = new MonitoredWebsitesBL();
            return bl.GetAllModifiedWebsites(username);
        }

        [WebMethod]
        public List<UserNotification> GetAllUserNotifications(string username)
        {
            NotificationsBL bl = new NotificationsBL();
            return bl.GetAllUserNotifications(username);
        }

        [WebMethod]
        public Notification GetNotification(string notificationId)
        {
            NotificationsBL bl = new NotificationsBL();
            return bl.GetNotification(notificationId);
        }

        [WebMethod]
        public void SolveUserNotification(string notificationId, string username)
        {
            NotificationsBL bl = new NotificationsBL();
            bl.SolveNotification(notificationId, username);
        }

        [WebMethod]
        public void InsertPendingTeacherRequest(string facultyUsername, string teacherUsername)
        {
            TeacherRequest request = new TeacherRequest
            {
                FacultyUsername = facultyUsername,
                TeacherUsername = teacherUsername,
                DateSent = DateTime.Now
            };
            TeacherRequestsBL bl = new TeacherRequestsBL();
            bl.InsertPendingTeacherRequest(request);
        }

        [WebMethod]
        public void SolvePendingTeacherRequest(string facultyUsername, string teacherUsername)
        {
            TeacherRequest request = new TeacherRequest
            {
                FacultyUsername = facultyUsername,
                TeacherUsername = teacherUsername,
                DateSent = DateTime.Now
            };
            TeacherRequestsBL bl = new TeacherRequestsBL();
            bl.SolveRequest(request);
        }
    }
}
