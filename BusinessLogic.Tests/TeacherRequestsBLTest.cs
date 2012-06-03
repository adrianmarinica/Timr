using System;
using System.Collections.Generic;
using NUnit.Framework;
using Objects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class TeacherRequestsBLTest
    {
        [Test]
        public void ShouldInsertRequest()
        {
            var teacherRequest = new TeacherRequest {FacultyUsername = "info.uaic", TeacherUsername = "rvlad", DateSent=DateTime.Now};
            var teacherRequestsBL = new TeacherRequestsBL();
            var requestsByFaculty = teacherRequestsBL.GetRequestsByFaculty("info.uaic");
            int length = -1;
            if (requestsByFaculty != null)
            {
                length = requestsByFaculty.Count;
            }

            teacherRequestsBL.InsertPendingTeacherRequest(teacherRequest);
            requestsByFaculty = teacherRequestsBL.GetRequestsByFaculty("info.uaic");
            if (requestsByFaculty != null)
            {
                Assert.AreEqual(requestsByFaculty[requestsByFaculty.Count - 1].FacultyUsername, "info.uaic");
                Assert.AreEqual(requestsByFaculty[requestsByFaculty.Count - 1].TeacherUsername, "rvlad");
                Assert.AreEqual(length + 1, requestsByFaculty.Count);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void ShouldSolveRequest()
        {
            var usersBL = new UsersBL();
            usersBL.InsertUser("busaco", "busaco", "busaco@info.uaic.ro", UserTypes.Teacher);
            usersBL.InsertUser("info.uaic", "info.uaic", "info.uaic", UserTypes.Faculty);

            var teacherRequestsBL = new TeacherRequestsBL();
            var teacherRequest = new TeacherRequest
                                     {
                                         DateSent = DateTime.Now,
                                         FacultyUsername = "info.uaic",
                                         TeacherUsername = "busaco"
                                     };
            var list = teacherRequestsBL.GetRequestsByFaculty("info.uaic");
            int length = -1;
            if (list != null) length = list.Count;
            teacherRequestsBL.InsertPendingTeacherRequest(teacherRequest);
            list = teacherRequestsBL.GetRequestsByFaculty("info.uaic");

            if (list != null) Assert.AreEqual(length + 1, list.Count);
            else Assert.Fail();

            teacherRequestsBL.SolveRequest(teacherRequest);

            list = teacherRequestsBL.GetRequestsByFaculty("info.uaic");

            // usersBL.DeleteUser("busaco", "busaco");
            // usersBL.DeleteUser("info.uaic", "info.uaic");

            if (list != null) Assert.AreEqual(length, list.Count);
            else Assert.Fail();
        }
    }
}