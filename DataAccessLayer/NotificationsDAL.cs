using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Objects;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;
using Constants.Tables;

namespace DataAccessLayer
{
    public class NotificationsDAL
    {
        // public void InsertTimetableNotification(TimetableNotification notification)
        // {
        //     NotificationsCollection.Collection.Insert<TimetableNotification>(notification);
        //     List<Student> students = UsersCollection.Collection.FindAllAs<Student>().ToList();
        //     
        //     foreach (var student in students)
        //     {
        //         foreach (var group in student.SubscribedGroups)
        //         {
        //             if (notification.ModifiedGroup.IsContainedIn(group))
        //             {
        //                 student.Notifications.Add(new UserNotification { NotificationId = notification._id.ToString(), UserSolved = false });
        //             }
        //         }
        //         UsersCollection.Collection.Save<Student>(student);
        //     }
        // }

        public void SolveNotification(string notificationId, string username)
        {
            var user = UsersCollection.Collection.FindOneByIdAs<User>(username);
            if(user != null)
            {
                if(user.UserType == UserTypes.Student)
                {
                    var student = user as Student;
                    if (student != null)
                    {
                        UserNotification userNotif = student.Notifications.Find(
                            delegate(UserNotification userNotification)
                                {
                                    return userNotification.NotificationId == notificationId;
                                });

                        if(userNotif != null)
                        {
                            userNotif.UserSolved = true;
                        }
                    }
                }
                else if(user.UserType == UserTypes.Teacher)
                {
                    var teacher = user as Teacher;
                    if (teacher != null)
                    {
                        UserNotification userNotif = teacher.Notifications.Find(
                            delegate(UserNotification userNotification)
                                {
                                    return userNotification.NotificationId == notificationId;
                                });
                        if(userNotif != null)
                        {
                            userNotif.UserSolved = true;
                        }
                    }
                }
            }
        }

        public void InsertNotification(Notification notification)
        {
            NotificationsCollection.Collection.Insert(notification);
        }

        public Notification GetNotification(string notificationId, NotificationTypes notificationType)
        {
            switch (notificationType)
            {
                case NotificationTypes.FeedNotification:
                    return NotificationsCollection.Collection.FindOneByIdAs<FeedNotification>(new ObjectId(notificationId));
                case NotificationTypes.TimetableNotification:
                    return NotificationsCollection.Collection.FindOneByIdAs<TimetableNotification>(new ObjectId(notificationId));
                case NotificationTypes.MonitoredWebsitesNotification:
                    return NotificationsCollection.Collection.FindOneByIdAs<MonitoredWebsiteNotification>(new ObjectId(notificationId));
            }
            return null;
        }
    }
}
