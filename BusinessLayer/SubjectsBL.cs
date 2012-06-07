using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class SubjectsBL
    {
        private SubjectsDAL dal
        {
            get
            {
                return new SubjectsDAL();
            }
        }

        public Subject GetSubject(string id)
        {
            return dal.GetSubject(id);
        }

        public void InsertSubject(Subject subject)
        {
            dal.InsertSubject(subject);
        }

        public List<Subject> GetAllSubjects()
        {
            return dal.GetAllSubjects();
        }

        public string GetFailedAndOptionalSubjectsAndWebsitesAsXml(string username)
        {
            var usersBL = new UsersBL();
            var student = usersBL.GetStudent(username);
            var subjectsBL = new SubjectsBL();

            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var subjects = new XElement("subjects");

            if(student != null)
            {
                if (student.OptionalSubjects != null)
                {
                    var optionalSubjects = new XElement("optionalSubjects");
                    foreach (var optionalSubject in student.OptionalSubjects)
                    {
                        var optionalSubjectElement = new XElement("optionalSubject");
                        optionalSubjectElement.Add(new XElement("id", optionalSubject));

                        var subject = subjectsBL.GetSubject(optionalSubject);
                        if (subject != null && subject.Name != null)
                        {
                            optionalSubjectElement.Add(new XElement("name", subject.Name));
                            if (subject.Websites != null)
                            {
                                var websitesElement = new XElement("websites");
                                foreach (var website in subject.Websites)
                                {
                                    websitesElement.Add(new XElement("website", website));
                                }
                                optionalSubjectElement.Add(websitesElement);
                            }
                        }

                        optionalSubjects.Add(optionalSubjectElement);
                    }
                    subjects.Add(optionalSubjects);
                }
                
                if (student.FailedSubjects != null)
                {
                    var failedSubjects = new XElement("failedSubjects");
                    foreach (var failedSubject in student.FailedSubjects)
                    {
                        var failedSubjectElement = new XElement("failedSubject");
                        failedSubjectElement.Add(new XElement("id", failedSubject));

                        var subject = subjectsBL.GetSubject(failedSubject);
                        if (subject != null && subject.Name != null)
                        {
                            failedSubjectElement.Add(new XElement("name", subject.Name));
                            if (subject.Websites != null)
                            {
                                var websitesElement = new XElement("websites");
                                foreach (var website in subject.Websites)
                                {
                                    websitesElement.Add(new XElement("website", website));
                                }
                                failedSubjectElement.Add(websitesElement);
                            }
                        }
                        failedSubjects.Add(failedSubjectElement);
                        subjects.Add(failedSubjects);
                    }
                }
                
            }
            document.Add(subjects);
            return document.ToString();
        }

        public string GetAllSubjectsAndWebsitesAsXml()
        {
            var allSubjects = dal.GetAllSubjects();
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var subjects = new XElement("subjects");

            if (allSubjects != null)
            {
                foreach (var subject in allSubjects)
                {
                    var subjectElement = new XElement("subject");
                    var subjectName = new XElement("name", subject.Name);
                    subjectElement.Add(subjectName);

                    if (subject.Websites != null)
                    {
                        var websitesElement = new XElement("websites");
                        foreach (var website in subject.Websites)
                        {
                            websitesElement.Add(new XElement("website", website));
                        }
                        subjectElement.Add(websitesElement);
                    }
                    subjects.Add(subjectElement);
                }
            }
            document.Add(subjects);
            return document.ToString();
        }
    }
}
