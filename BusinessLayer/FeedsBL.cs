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

        public List<Feed> GetFeedsForGroup(Group group)
        {
            return dal.GetFeedsForGroup(group);
        }

        public void InsertFeed(Feed feed)
        {
            dal.InsertFeed(feed);
        }
    }
}
