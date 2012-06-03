using System;
using System.Collections.Generic;
using NUnit.Framework;
using Objects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class FeedsBLTest
    {
        [Test]
        public void ShouldInsertFeeds()
        {
            var bl = new FeedsBL();

            var list = bl.GetFeedsForGroup(new Group
            {
                YearOfStudy = StudyYear.I2, HalfYearOfStudy = HalfYear.B
            });

            int length = -1;
            if (list != null)
            {
                length = list.Count;
            }
            var feed = new Feed
            {
                Message = "It started raining.",
                PublishDate = new DateTime(2012, 2, 2),
                ReceiverGroups = new List<Group>{ new Group { HalfYearOfStudy = HalfYear.B, YearOfStudy = StudyYear.I2}}
            };


            bl.InsertFeed(feed);

            feed = new Feed
                       {
                           Message = "It started raining.",
                           PublishDate = new DateTime(2012, 2, 2),
                           ReceiverGroups =
                               new List<Group> {new Group {HalfYearOfStudy = HalfYear.B, YearOfStudy = StudyYear.I2}}
                       };
            bl.InsertFeed(feed);
            list = bl.GetFeedsForGroup(new Group{YearOfStudy=StudyYear.I2, HalfYearOfStudy =  HalfYear.B});
            if (list == null) Assert.Fail();
            Assert.AreEqual(length + 2, list.Count);
        }
    }
}
