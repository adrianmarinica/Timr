using System;
using System.Web.Services;
using FIITimetableParser.FiiObjects;
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

    public class TimrService : WebService
    {
        // [WebMethod]
        // public string GetFiiTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        // {
        //     var bl = new FiiTimetableBL();
        //     return bl.GetXMLTimetableForBachelorYear(year, halfYear);
        // }
        // 
        // [WebMethod]
        // public string GetFiiTimetableForMastersYear(StudyYear year)
        // {
        //     var bl = new FiiTimetableBL();
        //     return bl.GetXMLTimetableForMastersYear((StudyYear)year);
        // }

        [WebMethod]
        public bool InsertUser(string username, string password, string email, UserTypes userType)
        {
            var bl = new UsersBL();
            return bl.InsertUser(username, password, email, userType);
        }

        [WebMethod]
        public bool ValidateUser(string username, string password)
        {
            var bl = new UsersBL();
            var ok = bl.ValidateUser(username, password, UserTypes.Student);
            return ok || bl.ValidateUser(username, password, UserTypes.Teacher);
        }

        [WebMethod]
        public void DeleteUser(string username, string password)
        {
            var bl = new UsersBL();
            bl.DeleteUser(username, password);
        }

        [WebMethod]
        public string GetAllMonitoredWebsitesAsXml(string username)
        {
            var bl = new MonitoredWebsitesBL();
            return bl.GetAllSubscribedWebsitesAsXml(username);
        }

        [WebMethod]
        public void SolveUserNotification(string notificationId, string username)
        {
            var bl = new NotificationsBL();
            bl.SolveNotification(notificationId, username);
        }

        [WebMethod]
        public void InsertPendingTeacherRequest(string facultyUsername, string teacherUsername)
        {
            var request = new TeacherRequest
            {
                FacultyUsername = facultyUsername,
                TeacherUsername = teacherUsername,
                DateSent = DateTime.Now
            };
            var bl = new TeacherRequestsBL();
            bl.InsertPendingTeacherRequest(request);
        }

        [WebMethod]
        public void SolvePendingTeacherRequest(string facultyUsername, string teacherUsername)
        {
            var request = new TeacherRequest
            {
                FacultyUsername = facultyUsername,
                TeacherUsername = teacherUsername,
                DateSent = DateTime.Now
            };
            var bl = new TeacherRequestsBL();
            bl.SolveRequest(request);
        }

        [WebMethod]
        public string GetStudentAsXml(string username)
        {
            var bl = new UsersBL();
            return bl.GetStudentAsXml(username);
        }

        [WebMethod]
        public string GetTimetableForUser(string username, string faculty)
        {
            var bl = new UsersBL();
            return bl.GetTimetableForUserAsXml(username, faculty);
        }

        // [WebMethod]
        // public string GetSubjectsAndWebsitesAsXml(string username)
        // {
        //     var bl = new SubjectsBL();
        //     return bl.GetFailedAndOptionalSubjectsAndWebsitesAsXml(username);
        // }

        [WebMethod]
        public string GetStudentNotificationsAsXml(string username, int count)
        {
            var bl = new NotificationsBL();
            return bl.GetUserNotificationsAsXml(username, count, UserTypes.Student);
        }

        [WebMethod]
        public string GetTeacherNotificationsAsXml(string username, int count)
        {
            var bl = new NotificationsBL();
            return bl.GetUserNotificationsAsXml(username, count, UserTypes.Teacher);
        }

        // [WebMethod]
        // public void InsertFeed(string username, string faculty, string message)
        // {
        //     var feed = new Feed();
        //     //feed.
        // }
    }
}
