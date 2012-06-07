using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class FeedsBL
    {
        private FeedsDAL dal
        {
            get
            {
                return new FeedsDAL();
            }
        }

        public List<Feed> GetFeedsForUser(string username)
        {
            return dal.GetFeedsForUser(username);
        }

        public List<Feed> GetFeedsForGroup(string facultyId, string groupId)
        {
            return dal.GetFeedsForGroup(facultyId, groupId);
        }

        public void InsertFeed(Feed feed)
        {
            dal.InsertFeed(feed);
        }

        public Feed GetFeed(string feedId)
        {
            return dal.GetFeed(feedId);
        }
    }
}
