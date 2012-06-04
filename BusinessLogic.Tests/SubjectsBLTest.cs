using System.Collections.Generic;
using NUnit.Framework;
using Objects;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class SubjectsBLTest
    {
        [Test]
        public void ShouldInsertSubject()
        {
            var bl = new SubjectsBL();
            var subject = new Subject
                              {
                                  Name = "Tehnologii Web",
                                  IsOptional = true,
                                  Websites = new List<string> {"http://thor.info.uaic.ro/~busaco/"},
                                  TeacherList = new List<string> {"busaco"}
                              };
            bl.InsertSubject(subject);
            subject = new Subject
                          {
                              Name = "Arhitectura Calculatoarelor",
                              IsOptional = true,
                              Websites = new List<string> {"http://thor.info.uaic.ro/~rvlad/"},
                              TeacherList = new List<string> {"rvlad"}
                          };
            bl.InsertSubject(subject);
        }
    }
}