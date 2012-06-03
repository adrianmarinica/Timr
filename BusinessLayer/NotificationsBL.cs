using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer;

namespace BusinessLogic
{
    public class NotificationsBL
    {
        public NotificationsDAL dal
        {
            get
            {
                return new NotificationsDAL();
            }
        }

        public void InsertNotification(Notification notification)
        {
            dal.InsertNotification(notification);
        }

        public void SolveNotification(string notificationId, string username)
        {
            dal.SolveNotification(notificationId, username);
        }

        public List<UserNotification> GetAllUserNotifications(string username)
        {
            UsersBL usersBL = new UsersBL();
            User user = usersBL.GetUser(username);
            List<UserNotification> list = null;
            if (user != null && user.UserType == UserTypes.Teacher)
            {
                var teacher = user as Teacher;
                if (teacher != null) list = teacher.Notifications;
            }
            if(user != null && user.UserType == UserTypes.Student)
            {
                var student = user as Student;
                if (student != null) list = student.Notifications;
            }
            return list;
        }

        public Notification GetNotification(string notificationId)
        {
            return dal.GetNotification(notificationId);
        }
    }
}
