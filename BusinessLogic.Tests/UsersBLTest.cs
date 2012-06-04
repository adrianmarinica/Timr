using System.Collections.Generic;
using NUnit.Framework;
using Objects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class UsersBLTest
    {
        [Test]
        public void ShouldDeleteUser()
        {
            var bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", UserTypes.Student);
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            User user = bl.GetUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsNull(user);
        }

        [Test]
        public void ShouldInsertComplexStudent()
        {
            var student = new Student
                              {
                                  Email = "apmarinica@gmail.com",
                                  Name = "Adrian Marinica",
                                  Id = "Adrian3",
                                  IsApproved = true,
                                  IsLockedOut = false,
                                  Notifications = new List<UserNotification>
                                                      {
                                                          new UserNotification
                                                              {NotificationId = "1", UserSolved = false},
                                                          new UserNotification
                                                              {NotificationId = "2", UserSolved = false}
                                                      },
                                  SubscribedWebsites = new List<string>
                                                           {
                                                               "http://info.uaic.ro/~busaco/",
                                                               "http://info.uaic.ro/~rvlad/"
                                                           },
                                  SubscribedGroups = new List<Group>
                                                         {
                                                             new Group
                                                                 {
                                                                     YearOfStudy = StudyYear.I2,
                                                                     HalfYearOfStudy = HalfYear.B,
                                                                     Number = "7"
                                                                 },
                                                             new Group
                                                                 {
                                                                     YearOfStudy = StudyYear.I2,
                                                                     HalfYearOfStudy = HalfYear.A,
                                                                     Number = "7"
                                                                 }
                                                         },
                                  SubscribedFaculties = new List<string>
                                                            {
                                                                "info.uaic"
                                                            },
                                  FailedSubjects = new List<string>
                                                       {
                                                           "4fcd249affd8e810ecc3c15f"
                                                       },
                                  OptionalSubjects = new List<string>
                                                         {
                                                             "4fcd249affd8e810ecc3c15e"
                                                         }
                              };
            var usersBL = new UsersBL();
            usersBL.InsertUser(student);
        }

        [Test]
        public void ShouldInsertUserOnlyOnceAndThenDeleteIt()
        {
            var bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", UserTypes.Student);
            bool actual = bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", UserTypes.Student);
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsFalse(actual);
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
        }

        [Test]
        public void ShouldValidateUser()
        {
            var bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", UserTypes.Student);
            Assert.IsTrue(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei111"));
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
        }
    }
}