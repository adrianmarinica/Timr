using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Xml.Linq;
using FIITimetableParser;
using Objects;
using DataAccessLayer;

namespace BusinessLogic
{
    public class UsersBL
    {
        private UsersDAL dal
        {
            get
            {
                return new UsersDAL();
            }
        }
        public bool InsertUser(string username, string password, string email, UserTypes userType)
        {
            return dal.InsertUser(new User
            {
                Id = username,
                Password = password,
                Email = email,
                UserType = userType
            });
        }

        public bool ValidateUser(string username, string password, UserTypes type)
        {
            return dal.ValidateUser(username, password, type);
        }

        public void DeleteUser(string username, string password)
        {
            dal.DeleteUser(username, password);
        }

        public void AddSubscribedWebsite(string userName, string websiteLink, string websiteOwner)
        {
            if (!String.IsNullOrEmpty(websiteLink))
            {
                dal.AddSubscribedWebsite(userName, ParseLink(websiteLink));
                var bl = new MonitoredWebsitesBL();
                var website = new MonitoredWebsite
                {
                    Id = websiteLink,
                    Title = websiteOwner
                };
                bl.SaveMonitoredWebsite(website);
            }
        }

        private string ParseLink(string rawLink)
        {
            if (!rawLink.StartsWith("http://"))
            {
                rawLink = String.Format("http://{0}", rawLink);
            }
            if (!rawLink.EndsWith("/"))
            {
                rawLink = String.Format("{0}/", rawLink);
            }
            return rawLink;
        }

        public User GetUser(string username, string password)
        {
            return dal.GetUser(username, password);
        }

        public User GetUser(string username)
        {
            return dal.GetUser(username);
        }

        public List<Teacher> GetTeachersByFaculty(string facultyUserName)
        {
            return dal.GetTeachersByFaculty(facultyUserName);
        }

        public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return dal.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
        }

        public User InsertUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return dal.InsertUser(username, password, email, passwordQuestion, passwordAnswer, isApproved,
                                  providerUserKey, out status);
        }

        public string GetPassword(string username, string answer)
        {
            return dal.GetPassword(username, answer);
        }

        public Student GetStudent(string username)
        {
            return dal.GetUser(username, UserTypes.Student) as Student;
        }

        public string GetStudentAsXml(string username)
        {
            User user = dal.GetUser(username, UserTypes.Student);
            Student student;
            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var studentElement = new XElement("student");
            if(user != null && (student = user as Student) != null)
            {
                var name = new XElement("name", student.Name);
                var email = new XElement("email", student.Email);
                var userName = new XElement("username", student.Id);
                var faculties = new XElement("faculties");
                if (student.SubscribedFaculties != null)
                    foreach (var faculty in student.SubscribedFaculties)
                    {
                        faculties.Add(new XElement("faculty", faculty));
                    }
                // var websites = new XElement("websites");
                // if (student.SubscribedWebsites != null)
                // {
                //     var monitoredWebsitesBL = new MonitoredWebsitesBL();
                //     foreach (var website in student.SubscribedWebsites)
                //     {
                //         var websiteObject = monitoredWebsitesBL.GetWebsite(website);
                //         if (websiteObject != null)
                //         {
                //             var websiteElement = new XElement("website");
                // 
                //             var websiteName = new XElement("name");
                //             if (websiteObject.Title != null) websiteName.Value = websiteObject.Title;
                // 
                //             var websiteLink = new XElement("link");
                //             if (websiteObject.Id != null) websiteLink.Value = websiteObject.Id;
                // 
                //             var websiteOwner = new XElement("owner");
                //             if (websiteObject.Owner != null)
                //                 websiteOwner.Value = websiteObject.Owner;
                // 
                //             websiteElement.Add(websiteName);
                //             websiteElement.Add(websiteLink);
                //             websiteElement.Add(websiteOwner);                            
                // 
                //             websites.Add(new XElement("website", websiteElement));
                //         }
                //     }
                // }
                var groups = new XElement("groups");
                if (student.SubscribedGroups != null)
                    foreach (var groupElement in student.SubscribedGroups)
                    {
                        var group = new XElement("group");
                        group.Value = groupElement;
                        groups.Add(group);
                    }
                var subjectsBL = new SubjectsBL();

                var optionalSubjects = new XElement("optionalSubjects");
                if(student.OptionalSubjects != null)
                {
                    foreach (var optionalSubject in student.OptionalSubjects)
                    {
                        var optionalSubjectElement = new XElement("optionalSubject");
                        optionalSubjectElement.Add(new XElement("id", optionalSubject));

                        var subject = subjectsBL.GetSubject(optionalSubject);
                        if (subject != null && subject.Name != null)
                            optionalSubjectElement.Add(new XElement("name", subject.Name));
                        optionalSubjects.Add(optionalSubjectElement);
                    }
                }
                var failedSubjects = new XElement("failedSubjects");
                if (student.FailedSubjects != null)
                    foreach (var failedSubject in student.FailedSubjects)
                    {
                        var failedSubjectElement = new XElement("failedSubject");
                        failedSubjectElement.Add(new XElement("id", failedSubject));

                        var subject = subjectsBL.GetSubject(failedSubject);
                        if (subject != null && subject.Name != null)
                            failedSubjectElement.Add(new XElement("name", subject.Name));
                        failedSubjects.Add(failedSubjectElement);
                    }

                studentElement.Add(name);
                studentElement.Add(email);
                studentElement.Add(userName);
                studentElement.Add(faculties);
                // studentElement.Add(websites);
                studentElement.Add(groups);
                studentElement.Add(optionalSubjects);
                studentElement.Add(failedSubjects);
            }
            xDocument.Add(studentElement);
            return xDocument.ToString();
        }

        public string ConvertToXML(Timetable timetable, List<Subject> availableSubjects)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var rootTimetable = new XElement("timetable");

            var monday = new XElement("monday");
            var tuesday = new XElement("tuesday");
            var wednesday = new XElement("wednesday");
            var thursday = new XElement("thursday");
            var friday = new XElement("friday");
            var saturday = new XElement("saturday");
            var sunday = new XElement("sunday");

            foreach (var item in timetable.TimetableItems)
            {
                var timetableSubitem = new XElement("tableItem");
                if (availableSubjects != null)
                {
                    var subjectItem =
                        availableSubjects.Find(delegate(Subject subject) { return subject.Name == item.ClassName; });
                    if (subjectItem != null)
                        timetableSubitem.Add(new XAttribute("id", subjectItem._id.ToString()));
                }
                timetableSubitem.Add(new XElement("startTime", item.StartTime));
                timetableSubitem.Add(new XElement("endTime", item.EndTime));
                timetableSubitem.Add(new XElement("className", item.ClassName));
                timetableSubitem.Add(new XElement("classType", (int)item.TypeOfClass));
                timetableSubitem.Add(new XElement("teacherName", item.Teacher));
                timetableSubitem.Add(new XElement("roomNumber", item.RoomNumber));
                timetableSubitem.Add(new XElement("frequency", (int)item.Frequency));
                timetableSubitem.Add(new XElement("optionalPackage", item.OptionalPackage));

                var groups = new XElement("groups");

                foreach (var groupItem in item.StudyGroups)
                {
                    string group = GetGroupName(groupItem, timetable.Faculty);
                    var parser = new Parser();
                    var fiiGroupItem = parser.GetGroupFromCell(group);
                    var groupElement = new XElement("group");
                    groupElement.Add(new XElement("yearOfStudy", (int)fiiGroupItem.YearOfStudy));
                    groupElement.Add(new XElement("halfYearOfStudy", fiiGroupItem.HalfYearOfStudy));
                    groupElement.Add(new XElement("groupNumber", fiiGroupItem.Number));
                    groups.Add(groupElement);
                }

                timetableSubitem.Add(groups);

                switch (item.Day)
                {
                    case DayOfWeek.Friday:
                        friday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Monday:
                        monday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Saturday:
                        saturday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Sunday:
                        sunday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Thursday:
                        thursday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Tuesday:
                        tuesday.Add(timetableSubitem);
                        break;
                    case DayOfWeek.Wednesday:
                        wednesday.Add(timetableSubitem);
                        break;
                }
            }

            rootTimetable.Add(monday);
            rootTimetable.Add(tuesday);
            rootTimetable.Add(wednesday);
            rootTimetable.Add(thursday);
            rootTimetable.Add(friday);
            rootTimetable.Add(saturday);
            rootTimetable.Add(sunday);
            document.Add(rootTimetable);
            return document.ToString();
        }

        public string GetTimetableForUserAsXml(string username, string faculty)
        {
            var fullTimetable = GetTimetableForUser(username, faculty);
            var subjectsBL = new SubjectsBL();
            return ConvertToXML(fullTimetable, subjectsBL.GetAllSubjects());
        }
        
        public Timetable GetTimetableForUser(string username, string faculty)
        {
            var tDal = new TimetablesDAL();
            var student = GetStudent(username);
            var fullTimetable = new Timetable();

            if (student != null && student.SubscribedGroups != null)
            {
                if (student.SubscribedFaculties != null && student.SubscribedFaculties.IndexOf(faculty) != -1)
                {
                    fullTimetable.Faculty =
                        student.SubscribedFaculties.ElementAt(student.SubscribedFaculties.IndexOf(faculty));
                    foreach (var subscribedGroup in student.SubscribedGroups)
                    {
                        var timetableForFinalGroup = tDal.GetTimetableForFinalGroup(subscribedGroup, faculty);
                        fullTimetable.TimetableItems.AddRange(timetableForFinalGroup);
                    }
                }
            }
            return fullTimetable;
        }

        public void InsertUser(Student student)
        {
            dal.InsertUser(student);
        }

        public void UpdateUserType(int type, string username)
        {
            dal.UpdateUserType((UserTypes)type, username);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return dal.ChangePassword(username, oldPassword, newPassword);
        }

        public Teacher GetTeacher(string username)
        {
            return (Teacher)dal.GetUser(username, UserTypes.Teacher);
        }

        public void SetFacultyGroups(string facultyName, GenericGroup group)
        {
            dal.SetFacultyGroups(facultyName, group);
        }

        public Faculty GetFaculty(string username)
        {
            return (Faculty) dal.GetUser(username, UserTypes.Faculty);
        }

        public List<string> GetGroupsByFaculty(string facultyId)
        {
            return dal.GetGroupsByFaculty(facultyId);
        }

        public void SaveStudent(Student student)
        {
            dal.SaveStudent(student);
        }

        public string GetGroupName(string groupId, string facultyId)
        {
            return dal.GetGroupName(facultyId, groupId);
        }

        public string GetGroupIdByName(string facultyId, string groupName)
        {
            return dal.GetGroupIdByName(facultyId, groupName);
        }

        public List<Student> GetAllStudents()
        {
            return dal.GetAllStudents();
        }
    }
}