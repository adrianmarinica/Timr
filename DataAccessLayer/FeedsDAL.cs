using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;

namespace DataAccessLayer
{
    public class FeedsDAL
    {
        public List<Feed> GetFeedsForGroup(Group group)
        {
            List<Feed> list = null;
            if (group.YearOfStudy != StudyYear.None)
            {
                if (!String.IsNullOrEmpty(group.Number))
                {
                    if ((group.HalfYearOfStudy == HalfYear.None &&
                        (group.YearOfStudy != StudyYear.I1 || group.YearOfStudy != StudyYear.I2 || group.YearOfStudy != StudyYear.I3)) || // masters or bachelors

                        (group.HalfYearOfStudy != HalfYear.None &&
                        (group.YearOfStudy == StudyYear.I1 || group.YearOfStudy == StudyYear.I2 || group.YearOfStudy == StudyYear.I3)))
                    {
                        list = GetFeedsForFullGroup(group);
                    }
                }
                else
                    if (group.HalfYearOfStudy != HalfYear.None)
                    {
                        list = GetFeedsForHalfYear(group.YearOfStudy, group.HalfYearOfStudy);
                    }
                    else
                        list = GetFeedsForYear(group.YearOfStudy);
            }
            return list;
        }

        private List<Feed> GetFeedsForYear(StudyYear yearOfStudy)
        {
            var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", yearOfStudy);
            return FeedsCollection.Collection.FindAs<Feed>(yearQuery).ToList();
        }

        private List<Feed> GetFeedsForHalfYear(StudyYear yearOfStudy, HalfYear halfYearOfStudy)
        {
            var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", yearOfStudy);
            var halfYearQuery = Query.EQ("ReceiverGroups.HalfYearOfStudy", halfYearOfStudy);
            var fullQuery = Query.And(yearQuery, halfYearQuery);

            return FeedsCollection.Collection.FindAs<Feed>(fullQuery).ToList();
        }

        private List<Feed> GetFeedsForFullGroup(Group group)
        {
            var yearQuery = Query.EQ("ReceiverGroups.YearOfStudy", group.YearOfStudy);
            var halfYearQuery = Query.EQ("ReceiverGroups.HalfYearOfStudy", group.HalfYearOfStudy);
            var groupQuery = Query.EQ("ReceiverGroups.Number", group.Number);
            var fullQuery = Query.And(yearQuery, halfYearQuery, groupQuery);

            return FeedsCollection.Collection.FindAs<Feed>(fullQuery).ToList();
        }

        public void InsertFeed(Feed feed)
        {
            FeedsCollection.Collection.Insert<Feed>(feed);

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
                                if (subscribedGroup.IsContainedIn(feedGroup))
                                {
                                    feeds.Add(item);
                                }
                            }
                        }

                }
                if (item.ReceiverUsers.Find(
                    delegate (User actualUser)
                    {
                        return actualUser.Id == username;
                    }) != null)
                {
                    feeds.Add(item);
                }
            }
            return feeds;
        }
    }
}
