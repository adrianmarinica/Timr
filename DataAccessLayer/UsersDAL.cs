using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;
using Constants.Tables;

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
                UsersCollection.Collection.Insert(typeof(User), user);
                return true;
            }
            return false;
        }
        public void DeleteUser(string username, string password)
        {
            var deleteUsername = Query.EQ(Users.Id.Key, username);
            var deletePassword = Query.EQ(Users.Password.Key, password);
            var deleteUser = Query.And(deleteUsername, deletePassword);
            UsersCollection.Collection.Remove(deleteUser);
        }
        public bool ValidateUser(string username, string password)
        {
            var usernameExists = Query.EQ(Users.Id.Key, username);
            var passwordExists = Query.EQ(Users.Password.Key, password);
            var userIsValid = Query.And(usernameExists, passwordExists);

            if(UsersCollection.Collection.FindAs<Student>(userIsValid).Count() == 1)
            {
                return true;
            }
            return false;
        }
        public void UpdateUser(User user)
        {
            var updateUser = Query.EQ(Users.Id.Key, user.Id);
            var update = Update.Set(Users.Id.Key, user.Id).
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
        public User GetUser(string username, string password)
        {
            var usernameQuery = Query.EQ(Users.Id.Key, username);
            var passQuery = Query.EQ(Users.Password.Key, password);
            var userQuery = Query.And(usernameQuery, passQuery);

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
            Student user = UsersCollection.Collection.FindOneByIdAs<Student>(userName);
            if(user != null)
            {
                user.SubscribedWebsites.Add(websiteLink);
                UsersCollection.Collection.Save<Student>(user);
            }
        }
        public void RemoveSubscribedWebsite(string username, string websiteLink)
        {
            Student user = UsersCollection.Collection.FindOneByIdAs<Student>(username);
            if(user != null)
            {
                user.SubscribedWebsites.Remove(websiteLink);
                UsersCollection.Collection.Save<Student>(user);
            }
        }

        #endregion

        #region Teachers

        public List<Teacher> GetTeachersByFaculty(string facultyUserName)
        {
            Faculty faculty = UsersCollection.Collection.FindOneByIdAs<Faculty>(facultyUserName);
            if (faculty != null)
                return faculty.Teachers;
            else return null;
        }

        #endregion
    }
}