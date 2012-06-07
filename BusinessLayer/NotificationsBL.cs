using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class NotificationsBL
    {
        public NotificationsDAL dal
        {
            get { return new NotificationsDAL(); }
        }

        public void InsertNotification(Notification notification)
        {
            dal.InsertNotification(notification);
        }

        public void SolveNotification(string notificationId, string username)
        {
            dal.SolveNotification(notificationId, username);
        }

        private List<UserNotification> GetAllStudentNotifications(string username)
        {
            var usersBL = new UsersBL();
            Student student = usersBL.GetStudent(username);
            List<UserNotification> list = null;

            if (student != null)
            {
                list = student.Notifications;
            }
            return list;
        }

        private List<UserNotification> GetAllTeacherNotifications(string username)
        {
            var usersBL = new UsersBL();
            Teacher teacher = usersBL.GetTeacher(username);
            List<UserNotification> list = null;

            if (teacher != null)
            {
                list = teacher.Notifications;
            }
            return list;
        }

        public Notification GetNotification(string notificationId, NotificationTypes notificationType)
        {
            return dal.GetNotification(notificationId, notificationType);
        }

        public string GetUserNotificationsAsXml(string username, int count, UserTypes userType)
        {
            List<UserNotification> list = null;
            if (userType == UserTypes.Student)
                list = GetAllStudentNotifications(username);
            else if (userType == UserTypes.Teacher)
                list = GetAllTeacherNotifications(username);

            if (list != null)
            {
                if (count < list.Count)
                    list = list.GetRange(list.Count - count, count);
                return GetXmlFromNotifications(list);
            }
            return String.Empty;
        }

        private string GetXmlFromNotifications(List<UserNotification> list)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var notifications = new XElement("notifications");
            notifications.Add(new XAttribute("count", list.Count));
            var notificationsBL = new NotificationsBL();
            foreach (UserNotification userNotification in list)
            {
                var notificationElement = new XElement("notification");
                notificationElement.Add(new XElement("solved", userNotification.UserSolved));

                var notificationDetails = new XElement("details");
                notificationDetails.Add(new XAttribute("type", userNotification.NotificationType));
                Notification notification = null;
                switch (userNotification.NotificationType)
                {
                    case NotificationTypes.FeedNotification:

                        var feedNotification =
                            (FeedNotification)
                            notificationsBL.GetNotification(userNotification.NotificationId, NotificationTypes.FeedNotification);
                        if (feedNotification != null)
                        {
                            var feedbl = new FeedsBL();
                            Feed feed = feedbl.GetFeed(feedNotification.FeedId);
                            if (feed != null)
                            {
                                notificationDetails.Add(new XElement("message", feed.Message));
                                notificationDetails.Add(new XElement("sender", feed.Sender));
                            }
                            notification = feedNotification;
                        }
                        break;
                    case NotificationTypes.TimetableNotification:
                        var timetableNotification =
                            (TimetableNotification)
                            notificationsBL.GetNotification(userNotification.NotificationId, NotificationTypes.TimetableNotification);
                        if (timetableNotification != null)
                        {
                            notificationDetails.Add(new XElement("timetableNotificationType",
                                                                 timetableNotification.TimetableNotificationType));
                            notificationDetails.Add(new XElement("dayOfWeek", timetableNotification.Day));
                            notificationDetails.Add(new XElement("typeOfClass",
                                                                 timetableNotification.ModifiedItem.TypeOfClass));
                            notificationDetails.Add(new XElement("className",
                                                                 timetableNotification.ModifiedItem.ClassName));
                            notificationDetails.Add(new XElement("startTime",
                                                                 timetableNotification.ModifiedItem.StartTime));
                            notificationDetails.Add(new XElement("endTime",
                                                                 timetableNotification.ModifiedItem.EndTime));
                            notification = timetableNotification;
                        }
                        break;
                    case NotificationTypes.MonitoredWebsitesNotification:
                        var websiteNotification =
                            (MonitoredWebsiteNotification)notificationsBL.GetNotification(userNotification.NotificationId, NotificationTypes.MonitoredWebsitesNotification);
                        if (websiteNotification != null)
                        {
                            notificationDetails.Add(new XElement("websiteLink", websiteNotification.WebsiteId));
                            notification = websiteNotification;
                        }
                        break;
                }
                if (notification != null)
                {
                    notificationElement.Add(new XElement("dateSent", notification.SentDate.ToString()));
                    notificationElement.Add(new XElement("title", notification.Title));
                }
                notificationElement.Add(notificationDetails);

                notifications.Add(notificationElement);
            }
            document.Add(notifications);
            return document.ToString();
        }
    }
}