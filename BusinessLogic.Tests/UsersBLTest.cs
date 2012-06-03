using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BusinessLogic;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class UsersBLTest
    {
        [Test]
        public void ShouldInsertUserOnlyOnceAndThenDeleteIt()
        {
            UsersBL bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", Objects.UserTypes.Student);
            bool actual = bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", Objects.UserTypes.Student);
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsFalse(actual);
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
        }

        [Test]
        public void ShouldDeleteUser()
        {
            UsersBL bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", Objects.UserTypes.Student);
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            var user = bl.GetUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsNull(user);
        }

        [Test]
        public void ShouldValidateUser()
        {
            UsersBL bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", Objects.UserTypes.Student);
            Assert.IsTrue(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei111"));
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei"));
        }
    }
}
