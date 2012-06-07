using System;
using System.Collections.Generic;
using FIITimetableParser;
using FIITimetableParser.FiiObjects;
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
                                  SubscribedGroups = new List<string>
                                                         {
                                                             "asfsagasgagas",
                                                             "asdbsdvsdvwe"
                                                         },
                                  SubscribedFaculties = new List<string>
                                                            {
                                                                "info.uaic.ro"
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
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei", UserTypes.Student));
        }

        [Test]
        public void ShouldValidateUser()
        {
            var bl = new UsersBL();
            bl.InsertUser("ohgoahrgorei", "ohgoahrgorei", "ohgoahrgorei@mail.com", UserTypes.Student);
            Assert.IsTrue(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei", UserTypes.Student));
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei111", UserTypes.Student));
            bl.DeleteUser("ohgoahrgorei", "ohgoahrgorei");
            Assert.IsFalse(bl.ValidateUser("ohgoahrgorei", "ohgoahrgorei", UserTypes.Student));
        }

        [Test]
        public void ShouldInsertGroups()
        {
            var usersBL = new UsersBL();
            var student = usersBL.GetStudent("Adrian2");
            student.SubscribedGroups = new List<string>
                                           {
                                               "cc0df0c3-6fac-4f65-98ac-fee10dcf38b7",
                                               "21c8d489-e9be-4127-864f-80e387ab3592"
                                           };
            usersBL.SaveStudent(student);
        }

        [Test]
        public void ShouldSetFIIFacultyGroups()
        {
            var bl = new UsersBL();
            var fullFaculty = new GenericGroup("FII");
            fullFaculty.Groups = new List<GenericGroup>();

            #region Bachelor
            var bachelor1 = new GenericGroup("I1");
            var bachelor1A = new GenericGroup("I1A");
            var bachelor1B = new GenericGroup("I1B");
            bachelor1A.Groups = new List<GenericGroup>();
            bachelor1B.Groups = new List<GenericGroup>();
            for (int i = 1; i < 8; i++)
            {
                bachelor1A.Groups.Add(new GenericGroup("I1A" + i));
                bachelor1B.Groups.Add(new GenericGroup("I1B" + i));    
            }
            bachelor1.Groups.Add(bachelor1A);
            bachelor1.Groups.Add(bachelor1B);

            var bachelor2 = new GenericGroup("I2");
            var bachelor2A = new GenericGroup("I2A");
            var bachelor2B = new GenericGroup("I2B");
            bachelor2A.Groups = new List<GenericGroup>();
            bachelor2B.Groups = new List<GenericGroup>();
            for (int i = 1; i < 8; i++)
            {
                bachelor2A.Groups.Add(new GenericGroup("I2A" + i));
                bachelor2B.Groups.Add(new GenericGroup("I2B" + i));
            }
            bachelor2.Groups.Add(bachelor2A);
            bachelor2.Groups.Add(bachelor2B);

            var bachelor3 = new GenericGroup("I3");
            var bachelor3A = new GenericGroup("I3A");
            var bachelor3B = new GenericGroup("I3B");
            bachelor3A.Groups = new List<GenericGroup>();
            bachelor3B.Groups = new List<GenericGroup>();
            for (int i = 1; i < 6; i++)
            {
                bachelor3A.Groups.Add(new GenericGroup("I3A" + i));
                bachelor3B.Groups.Add(new GenericGroup("I3B" + i));
            }
            bachelor3.Groups.Add(bachelor3B);
            bachelor3.Groups.Add(bachelor3B);

            fullFaculty.Groups.Add(bachelor1);
            fullFaculty.Groups.Add(bachelor2);
            fullFaculty.Groups.Add(bachelor3);
            #endregion

            #region Masters
            var mis1 = new GenericGroup("MIS1");
            mis1.Groups.Add(new GenericGroup("MIS11"));
            mis1.Groups.Add(new GenericGroup("MIS12"));
            var mis2 = new GenericGroup("MIS2");
            mis2.Groups.Add(new GenericGroup("MIS21"));
            mis2.Groups.Add(new GenericGroup("MIS22"));

            var mlc1 = new GenericGroup("MLC1");
            var mlc2 = new GenericGroup("MLC2");

            var moc1 = new GenericGroup("MOC1");
            var moc2 = new GenericGroup("MOC2");

            var msd1 = new GenericGroup("MSD1");
            var msd2 = new GenericGroup("MSD2");

            var msi1 = new GenericGroup("MSI1");
            var msi2 = new GenericGroup("MSI2");

            fullFaculty.Groups.Add(mis1);
            fullFaculty.Groups.Add(mis2);
            fullFaculty.Groups.Add(mlc1);
            fullFaculty.Groups.Add(mlc2);
            fullFaculty.Groups.Add(moc1);
            fullFaculty.Groups.Add(moc2);
            fullFaculty.Groups.Add(msd1);
            fullFaculty.Groups.Add(msd2);
            fullFaculty.Groups.Add(msi1);
            fullFaculty.Groups.Add(msi2);
            #endregion

            bl.SetFacultyGroups("info.uaic.ro", fullFaculty);
        }

        [Test]
        public void ShouldSetTimetableForFii()
        {
            var timetablesBL = new TimetablesBL();
            var usersBL = new UsersBL();
            List<string> list = usersBL.GetGroupsByFaculty("info.uaic.ro");

            foreach (var group in list)
            {
                Timetable timetable = new Timetable();
                timetable.Faculty = "info.uaic.ro";
                timetable.GroupId = group;
                string name = usersBL.GetGroupName(group, "info.uaic.ro");

                
                    StudyYear year = StudyYear.None;
                    HalfYear halfYear = HalfYear.None;
                    string number = null;

                    if(name.StartsWith("I")) // licenta
                    {
                        if(name.StartsWith("I1")) // lic 1
                        {
                            year = StudyYear.I1;
                        }
                        else if(name.StartsWith("I2")) // lic 2
                        {
                            year = StudyYear.I2;
                        }
                        else if(name.StartsWith("I3")) // lic 3
                        {
                            year = StudyYear.I3;
                        }

                        if (name.Length > 2)
                        {
                            switch (name[2])
                            {
                                case 'A':
                                    halfYear = HalfYear.A;
                                    break;
                                case 'B':
                                    halfYear = HalfYear.B;
                                    break;
                            }
                        }
                        number = name.Substring(3);
                    }
                    else if(name.StartsWith("M")) // master
                    {
                        halfYear = HalfYear.None;
                        if(name.StartsWith("MIS"))
                        {
                            switch (name[3])
                            {
                                case '1':
                                    year = StudyYear.MIS1;
                                    break;
                                case '2':
                                    year = StudyYear.MIS2;
                                    break;
                            }
                            number = name[4].ToString();
                        }
                        else if(name.StartsWith("MLC"))
                        {
                            switch (name[3])
                            {
                                case '1':
                                    year = StudyYear.MLC1;
                                    break;
                                case '2':
                                    year = StudyYear.MLC2;
                                    break;
                            }
                        }
                        else if (name.StartsWith("MOC"))
                        {
                            switch (name[3])
                            {
                                case '1':
                                    year = StudyYear.MOC1;
                                    break;
                                case '2':
                                    year = StudyYear.MOC2;
                                    break;
                            }
                        }
                        else if (name.StartsWith("MSD"))
                        {
                            switch (name[3])
                            {
                                case '1':
                                    year = StudyYear.MSD1;
                                    break;
                                case '2':
                                    year = StudyYear.MSD2;
                                    break;
                            }
                        }
                        else if (name.StartsWith("MSI"))
                        {
                            switch (name[3])
                            {
                                case '1':
                                    year = StudyYear.MSI1;
                                    break;
                                case '2':
                                    year = StudyYear.MSI2;
                                    break;
                            }
                        }
                    }
                    var parser = new Parser();
                    var timetableForGroup = parser.GetTimetableForGroup(year, halfYear, number);

                timetable.TimetableItems = ConvertToRegularTimetable(timetableForGroup);
                timetablesBL.SaveTimetable(timetable);
            }
        }

        private List<TimetableItem> ConvertToRegularTimetable(List<FiiTimetableItem> timetableForGroup)
        {
            var list = new List<TimetableItem>();

            foreach (var fiiTimetableItem in timetableForGroup)
            {
                var item = new TimetableItem();
                item.ClassName = fiiTimetableItem.ClassName;
                item.Day = fiiTimetableItem.Day;
                item.EndTime = fiiTimetableItem.EndTime;
                item.OptionalPackage = fiiTimetableItem.OptionalPackage;
                item.RoomNumber = fiiTimetableItem.RoomNumber;
                item.StartTime = fiiTimetableItem.StartTime;
                item.Frequency = fiiTimetableItem.Frequency;
                var studyGroupIds = new List<string>();
                foreach (var group in fiiTimetableItem.StudyGroups)
                {
                    string year = string.Empty;
                    if (group.YearOfStudy != StudyYear.None)
                        year = Enum.GetName(typeof (StudyYear), group.YearOfStudy);

                    string halfYear = String.Empty;
                    if (group.HalfYearOfStudy != HalfYear.None)
                        halfYear = Enum.GetName(typeof (HalfYear), group.HalfYearOfStudy);

                    studyGroupIds.Add(GetGroupIdByName(year + halfYear + group.Number));
                }
                item.StudyGroups = studyGroupIds;
                item.Teacher = fiiTimetableItem.TeacherName;
                item.TypeOfClass = fiiTimetableItem.TypeOfClass;
                list.Add(item);
            }
            return list;
        }

        private string GetGroupIdByName(string s)
        {
            var usersBL = new UsersBL();
            return usersBL.GetGroupIdByName("info.uaic.ro", s);
        }

        [Test]
        public void ShouldGetAllStudents()
        {
            UsersBL bl = new UsersBL();
            var list = bl.GetAllStudents();
        }
    }
}