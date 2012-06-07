using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Constants.Tables;
using DataAccessLayer.Collections;
using FIITimetableParser;
using Logger;
using MongoDB.Driver.Builders;
using Objects;

namespace DataAccessLayer
{
    public class UsersDAL
    {
        #region GenericUsers

        public bool InsertUser(User user)
        {
            var userObject = UsersCollection.Collection.FindOneByIdAs<User>(user.Id);
            if (userObject == null)
            {
                UsersCollection.Collection.Insert(typeof (User), user);
                return true;
            }
            return false;
        }

        public bool InserStudent(Student student)
        {
            var userObject = UsersCollection.Collection.FindOneByIdAs<Student>(student.Id);
            if (userObject == null)
            {
                UsersCollection.Collection.Insert(typeof (Student), student);
                return true;
            }
            return false;
        }

        public void DeleteUser(string username, string password)
        {
            QueryComplete deleteUsername = Query.EQ(Users.Username.Key, username);
            QueryComplete deletePassword = Query.EQ(Users.Password.Key, password);
            QueryComplete deleteUser = Query.And(deleteUsername, deletePassword);
            UsersCollection.Collection.Remove(deleteUser);
        }

        public bool ValidateUser(string username, string password, UserTypes type)
        {
            var usernameExists = Query.EQ("_id", username);
            var passwordExists = Query.EQ(Users.Password.Key, password);
            var userIsValid = Query.And(usernameExists, passwordExists);

            try
            {
                switch (type)
                {
                    case UserTypes.Sysop:
                        return UsersCollection.Collection.FindAs<Sysop>(userIsValid).Any();
                    case UserTypes.Faculty:
                        return UsersCollection.Collection.FindAs<Faculty>(userIsValid).Any();
                    case UserTypes.Teacher:
                        return UsersCollection.Collection.FindAs<Teacher>(userIsValid).Any();
                    case UserTypes.Student:
                        return UsersCollection.Collection.FindAs<Student>(userIsValid).Any();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex);
            }
            return false;
        }

        public void UpdateUser(User user)
        {
            QueryComplete updateUser = Query.EQ(Users.Username.Key, user.Id);
            UpdateBuilder update = Update.Set(Users.Username.Key, user.Id).
                Set(Users.Password.Key, user.Password).
                Set(Users.Email.Key, user.Email);

            UsersCollection.Collection.Update(updateUser, update);
        }

        public List<User> GetAllUsers()
        {
            return UsersCollection.Collection.FindAllAs<User>().ToList();
        }

        public User GetUser(string username)
        {
            return UsersCollection.Collection.FindOneByIdAs<User>(username);
        }

        public Student GetStudent(string username)
        {
            return UsersCollection.Collection.FindOneByIdAs<Student>(username);
        }

        public User GetUser(string username, string password)
        {
            QueryComplete usernameQuery = Query.EQ(Users.Username.Key, username);
            QueryComplete passQuery = Query.EQ(Users.Password.Key, password);
            QueryComplete userQuery = Query.And(usernameQuery, passQuery);

            return UsersCollection.Collection.FindOneAs<User>(userQuery);
        }

        #endregion

        #region StudentSubscribedWebsites

        public List<string> GetSubscribedWebsiteLinks(string userName)
        {
            return UsersCollection.Collection.FindOneByIdAs<Student>(userName).SubscribedWebsites;
        }

        public void AddSubscribedWebsite(string userName, string websiteLink)
        {
            var user = UsersCollection.Collection.FindOneByIdAs<Student>(userName);
            if (user != null)
            {
                user.SubscribedWebsites.Add(websiteLink);
                UsersCollection.Collection.Save(user);
            }
        }

        public void RemoveSubscribedWebsite(string username, string websiteLink)
        {
            var user = UsersCollection.Collection.FindOneByIdAs<Student>(username);
            if (user != null)
            {
                user.SubscribedWebsites.Remove(websiteLink);
                UsersCollection.Collection.Save(user);
            }
        }

        #endregion

        #region Teachers

        public List<Teacher> GetTeachersByFaculty(string facultyUserName)
        {
            var faculty = UsersCollection.Collection.FindOneByIdAs<Faculty>(facultyUserName);
            if (faculty != null)
            {
                if (faculty.Teachers != null)
                {
                    var list = new List<Teacher>();
                    foreach (var teacher in faculty.Teachers)
                    {
                        var teacherObject = GetUser(teacher, UserTypes.Teacher) as Teacher;
                        if (teacherObject != null)
                            list.Add(teacherObject);
                    }
                    return list;
                }
            }
            return null;
        }

        #endregion

        public bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                                    string newPasswordAnswer)
        {
            QueryComplete updateUser = Query.EQ(Users.Username.Key, username);
            UpdateBuilder update = Update.Set(Users.PasswordQuestion.Key, newPasswordQuestion).
                Set(Users.PasswordAnswer.Key, newPasswordAnswer);
            return UsersCollection.Collection.Update(updateUser, update).Ok;
        }

        public string GetPassword(string username, string answer)
        {
            var findOneByIdAs = UsersCollection.Collection.FindOneByIdAs<User>(username);
            return findOneByIdAs != null ? findOneByIdAs.Password : null;
        }

        public User InsertUser(string username, string password, string email, string passwordQuestion,
                               string passwordAnswer, bool isApproved, object providerUserKey,
                               out MembershipCreateStatus status)
        {
            var user = new User
                           {
                               Id = username,
                               Password = password,
                               Email = email,
                               PasswordQuestion = passwordQuestion,
                               PasswordAnswer = passwordAnswer,
                               IsApproved = isApproved
                           };

            var userObject = UsersCollection.Collection.FindOneByIdAs<User>(username);
            QueryComplete emailUnique = Query.EQ(Users.Email.Key, email);
            var userEmail = UsersCollection.Collection.FindOneAs<User>(emailUnique);
            if (userObject == null)
            {
                if (userEmail == null)
                {
                    UsersCollection.Collection.Insert(typeof (User), user);
                    status = MembershipCreateStatus.Success;
                }
                else
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                }
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return user;
        }

        public User GetUser(string username, UserTypes type)
        {
            switch (type)
            {
                case UserTypes.Sysop:
                    return UsersCollection.Collection.FindOneByIdAs<Sysop>(username);
                case UserTypes.Faculty:
                    return UsersCollection.Collection.FindOneByIdAs<Faculty>(username);
                case UserTypes.Teacher:
                    return UsersCollection.Collection.FindOneByIdAs<Teacher>(username);
                case UserTypes.Student:
                    return UsersCollection.Collection.FindOneByIdAs<Student>(username);
                default:
                    return null;
            }
        }

        //  public string GetTimetableForUserAsXml(string username)
        //  {
        //      User user = GetUser(username, UserTypes.Student);
        //      var exporter = new Exporter();
        //      var parser = new Parser();
        //      string result = String.Empty;
        //      var fullTimetable = new List<TimetableItem>();
        //      if (user != null)
        //      {
        //          var student = user as Student;
        //          if (student != null)
        //              if (student.SubscribedGroups != null)
        //                  foreach (Group group in student.SubscribedGroups)
        //                  {
        //                      List<TimetableItem> timetableForGroup = parser.GetTimetableForGroup(@group.YearOfStudy,
        //                                                                                          @group.HalfYearOfStudy,
        //                                                                                          @group.Number);
        //                      if (timetableForGroup != null) fullTimetable.AddRange(timetableForGroup);
        //                  }
        //      }
        //      var subjectsDAL = new SubjectsDAL();
        //      string convertToXML = exporter.ConvertToXML(fullTimetable, subjectsDAL.GetAllSubjects());
        //      if (convertToXML != null) result = convertToXML;
        //      return result;
        //  }

        public void SaveUser(User user)
        {
            UsersCollection.Collection.Save(user);
        }

        public void UpdateUserType(UserTypes userType, string username)
        {
            var user = UsersCollection.Collection.FindOneByIdAs<User>(username);
            if (user != null)
                user.UserType = userType;
            SaveUser(user);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = UsersCollection.Collection.FindOneByIdAs<User>(username);
            if (user.Password == oldPassword)
            {
                user.Password = newPassword;
                return true;
            }
            return false;
        }

        public void SetFacultyGroups(string facultyName, GenericGroup group)
        {
            var findOneByIdAs = UsersCollection.Collection.FindOneByIdAs<Faculty>(facultyName);
            findOneByIdAs.Groups = group;
            UsersCollection.Collection.Save(findOneByIdAs);
        }

        public List<string> GetGroupsByFaculty(string facultyId)
        {
            var user = GetUser(facultyId, UserTypes.Faculty);
            List<string> list = new List<string>();
            var faculty = user as Faculty;

            if (faculty != null && faculty.Groups != null && faculty.Groups.Groups != null)
            {
                foreach (var group in faculty.Groups.Groups) // year
                {
                    if (group.Groups != null && group.Groups.Count != 0)
                    {
                        foreach (var secondGroup in group.Groups) // half year
                        {
                            if (secondGroup.Groups != null && secondGroup.Groups.Count != 0)
                            {
                                foreach (var thirdGroup in secondGroup.Groups) // group
                                {
                                    list.Add(thirdGroup.Id.ToString());
                                }
                            }
                            else
                            {
                                list.Add(secondGroup.Id.ToString());
                            }
                        }
                    }
                    else
                    {
                        list.Add(group.Id.ToString());
                    }
                }
            }

            return list;
        }

        public void SaveStudent(Student student)
        {
            UsersCollection.Collection.Save(student);
        }

        public string GetGroupName(string facultyId, string groupId)
        {
            var faculty = GetUser(facultyId, UserTypes.Faculty) as Faculty;

            if (faculty != null && faculty.Groups != null && faculty.Groups.Groups != null)
            {
                foreach (var group in faculty.Groups.Groups) // year
                {
                    if(group.Id == groupId) return group.Name;

                    if (group.Groups != null && group.Groups.Count != 0)
                    {
                        foreach (var secondGroup in group.Groups) // half year
                        {
                            if (secondGroup.Id == groupId) return secondGroup.Name;

                            if (secondGroup.Groups != null && secondGroup.Groups.Count != 0)
                            {
                                foreach (var thirdGroup in secondGroup.Groups) // group
                                {
                                    if(thirdGroup.Id == groupId) return thirdGroup.Name;
                                }
                            }       
                        }
                    }
                }
            }
            return String.Empty;
        }

        public string GetGroupIdByName(string facultyName, string groupName)
        {
            var faculty = GetUser(facultyName, UserTypes.Faculty) as Faculty;

            if (faculty != null && faculty.Groups != null && faculty.Groups.Groups != null)
            {
                foreach (var group in faculty.Groups.Groups) // year
                {
                    if (group.Name == groupName) return group.Id;

                    if (group.Groups != null && group.Groups.Count != 0)
                    {
                        foreach (var secondGroup in group.Groups) // half year
                        {
                            if (secondGroup.Name == groupName) return secondGroup.Id;

                            if (secondGroup.Groups != null && secondGroup.Groups.Count != 0)
                            {
                                foreach (var thirdGroup in secondGroup.Groups) // group
                                {
                                    if (thirdGroup.Name == groupName) return thirdGroup.Id;
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        public List<Student> GetAllStudents()
        {
            var query = Query.EQ("UserType", 4);
            return UsersCollection.Collection.FindAs<Student>(query).ToList();
        }
    }

}