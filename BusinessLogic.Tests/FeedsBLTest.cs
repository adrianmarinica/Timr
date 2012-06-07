using System;
using System.Collections.Generic;
using NUnit.Framework;
using Objects;
using FIITimetableParser.FiiObjects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class FeedsBLTest
    {
        [Test]
        public void ShouldInsertFeeds()
        {
            var bl = new FeedsBL();

            var feed = new Feed
            {
                Sender = "busaco",
                Message = "It started raining.",
                PublishDate = new DateTime(2012, 2, 2),
                ReceiverGroups = new List<string> { "688936cd-b6ff-450a-b2ea-80573362e6de", "c920f326-5035-44d3-bd2f-690e2f831f1c" }
            };


            bl.InsertFeed(feed);

            feed = new Feed
                       {
                           Sender = "rvlad",
                           Faculty = "info.uaic.ro",
                           Message = "It started raining.",
                           PublishDate = new DateTime(2012, 2, 2),
                           ReceiverGroups = new List<string> { "0f44bd29-26ee-4c50-a14f-0e641e1af8c8", "400e8981-feb5-4291-aa61-3044ff52d5a6" }
                       };
            bl.InsertFeed(feed);

            // list = bl.GetFeedsForGroup("info.uaic.ro", "asfasgasgasg");
            // if (list == null) Assert.Fail();
            // Assert.AreEqual(length + 2, list.Count);
        }
    }
}
