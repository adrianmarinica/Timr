using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using Objects;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;

namespace DataAccessLayer
{
    public class FeedsDAL
    {
       // public List<Feed> GetFeedsForGroup(string facultyId, string groupId)
       // {
       //     List<Feed> list = null;
       //     var usersDAL = new UsersDAL();
       //     usersDAL.GetGroupsByFaculty(facultyId);
       //     if (group.YearOfStudy != StudyYear.None)
       //     {
       //         if (!String.IsNullOrEmpty(group.Number))
       //         {
       //             if ((group.HalfYearOfStudy == HalfYear.None &&
       //                 (group.YearOfStudy != StudyYear.I1 || group.YearOfStudy != StudyYear.I2 || group.YearOfStudy != StudyYear.I3)) || // masters or bachelors
       //
       //                 (group.HalfYearOfStudy != HalfYear.None &&
       //                 (group.YearOfStudy == StudyYear.I1 || group.YearOfStudy == StudyYear.I2 || group.YearOfStudy == StudyYear.I3)))
       //             {
       //                 list = GetFeedsForFullGroup(group);
       //             }
       //         }
       //         else
       //             if (group.HalfYearOfStudy != HalfYear.None)
       //             {
       //                 list = GetFeedsForHalfYear(group.YearOfStudy, group.HalfYearOfStudy);
       //             }
       //             else
       //                 list = GetFeedsForYear(group.YearOfStudy);
       //     }
       //     return list;
       // }
       //
       // private List<Feed> GetFeedsForYear(StudyYear yearOfStudy)
       // {
       //     var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", yearOfStudy);
       //     return FeedsCollection.Collection.FindAs<Feed>(yearQuery).ToList();
       // }
       //
       // private List<Feed> GetFeedsForHalfYear(StudyYear yearOfStudy, HalfYear halfYearOfStudy)
       // {
       //     var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", yearOfStudy);
       //     var halfYearQuery = Query.EQ("ReceiverGroups.HalfYearOfStudy", halfYearOfStudy);
       //     var fullQuery = Query.And(yearQuery, halfYearQuery);
       //
       //     return FeedsCollection.Collection.FindAs<Feed>(fullQuery).ToList();
       // }
       //
       // private List<Feed> GetFeedsForFullGroup(Group group)
       // {
       //     var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", group.YearOfStudy);
       //     var halfYearQuery = Query.EQ("ReceiverGroups.HalfYearOfStudy", group.HalfYearOfStudy);
       //     var groupQuery = Query.EQ("ReceiverGroups.Number", group.Number);
       //     var fullQuery = Query.And(yearQuery, halfYearQuery, groupQuery);
       //
       //     return FeedsCollection.Collection.FindAs<Feed>(fullQuery).ToList();
       // }

        public List<Feed> GetFeedsForGroup(string facultyId, string groupId)
        {
            var usersDAL = new UsersDAL();
            var feedsDAL = new FeedsDAL();
            var query = Query.EQ("ReceiverGroups.Id", groupId);

            return FeedsCollection.Collection.FindAs<Feed>(query).ToList();
        }

        public void InsertFeed(Feed feed)
        {
            FeedsCollection.Collection.Insert<Feed>(feed);
            
            var notificationsDAL = new NotificationsDAL();
            var notification = new FeedNotification
                                   {
                                       FeedId = feed._id.ToString(),
                                       NotificationType = NotificationTypes.FeedNotification,
                                       SentDate = DateTime.Now,
                                       Title = String.Format("{0} has posted a message.", feed.Sender)
                                   };
            notificationsDAL.InsertNotification(notification);

            var usersDAL = new UsersDAL();
            var allStudents = usersDAL.GetAllStudents();
            
                if (allStudents != null)
                    foreach (var student in allStudents)
                    {
                        if(student.SubscribedGroups.Find(delegate(string gr)
                                                             {
                                                                 foreach (var receiverGroup in feed.ReceiverGroups)
                                                                 {
                                                                     if (gr == receiverGroup)
                                                                         return true;
                                                                 }
                                                                 return false;
                                                             }

                        ).Any())
                        {
                            student.Notifications.Add(new UserNotification
                                                          {
                                                              NotificationId = notification._id.ToString(),
                                                              NotificationType = NotificationTypes.FeedNotification,
                                                              UserSolved = false
                                                          });
                        }
                        usersDAL.SaveStudent(student);
                    }
        }

        public List<Feed> GetFeedsForUser(string username)
        {
            List<Feed> feeds = FeedsCollection.Collection.FindAllAs<Feed>().ToList();

            foreach (var item in feeds)
            {
                var user = UsersCollection.Collection.FindOneByIdAs<User>(username);
                if (user.UserType == UserTypes.Student)
                {
                    var student = (user as Student);
                    if (student != null)
                        foreach (var subscribedGroup in student.SubscribedGroups)
                        {
                            foreach (var feedGroup in item.ReceiverGroups)
                            {
                                if (subscribedGroup == feedGroup)
                                {
                                    feeds.Add(item);
                                }
                            }
                        }
                }
                if (item.ReceiverUsers.Contains(username))
                {
                    feeds.Add(item);
                }
            }
            return feeds;
        }

        public Feed GetFeed(string feedId)
        {
            return FeedsCollection.Collection.FindOneByIdAs<Feed>(new ObjectId(feedId));
        }
    }
}
