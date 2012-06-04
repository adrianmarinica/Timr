using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml.Linq;
using MongoDB.Bson;
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

        public bool ValidateUser(string username, string password)
        {
            return dal.ValidateUser(username, password);
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
                MonitoredWebsitesBL bl = new MonitoredWebsitesBL();
                MonitoredWebsite website = new MonitoredWebsite
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

        public User GetStudent(string username)
        {
            return dal.GetUser(username, UserTypes.Student);
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
                var websites = new XElement("websites");
                if (student.SubscribedWebsites != null)
                {
                    var monitoredWebsitesBL = new MonitoredWebsitesBL();
                    foreach (var website in student.SubscribedWebsites)
                    {
                        var websiteObject = monitoredWebsitesBL.GetWebsite(website);
                        if (websiteObject != null)
                        {
                            var websiteElement = new XElement("website");

                            var websiteName = new XElement("name");
                            if (websiteObject.Title != null) websiteName.Value = websiteObject.Title;

                            var websiteLink = new XElement("link");
                            if (websiteObject.Id != null) websiteLink.Value = websiteObject.Id;

                            var websiteOwner = new XElement("owner");
                            if (websiteObject.Owner != null && websiteObject.Owner.Id != null)
                                websiteOwner.Value = websiteObject.Owner.Id;

                            var websiteSubject = new XElement("subject");
                            if (websiteObject.WebsiteSubject != null)
                                websiteSubject.Value = websiteObject.WebsiteSubject._id.ToString();
                            websiteElement.Add(websiteName);
                            websiteElement.Add(websiteLink);
                            websiteElement.Add(websiteOwner);
                            websiteElement.Add(websiteSubject);

                            websites.Add(new XElement("website", websiteElement));
                        }
                    }
                }
                var groups = new XElement("groups");
                if (student.SubscribedGroups != null)
                    foreach (var groupElement in student.SubscribedGroups)
                    {
                        var group = new XElement("group");
                        group.Add(new XElement("yearOfStudy", (int)groupElement.YearOfStudy));
                        group.Add(new XElement("halfYearOfStudy", groupElement.HalfYearOfStudy));
                        group.Add(new XElement("groupNumber", groupElement.Number));
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
                studentElement.Add(websites);
                studentElement.Add(groups);
                studentElement.Add(optionalSubjects);
                studentElement.Add(failedSubjects);
            }
            xDocument.Add(studentElement);
            return xDocument.ToString();
        }

        public string GetTimetableForUserAsXml(string username)
        {
            return dal.GetTimetableForUserAsXml(username);
        }

        public void InsertUser(Student student)
        {
            dal.InsertUser(student);
        }
    }
}