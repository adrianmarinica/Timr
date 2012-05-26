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
        public bool InsertUser(User user)
        {
            var userExists = Query.Exists("Username", true);

            if (UsersCollection.Collection.FindAs<User>(userExists).Count() == 0)
            {
                UsersCollection.Collection.Insert(typeof(User), user);
                return true;
            }
            return false;
        }

        public void DeleteUser(string username, string password)
        {
            var deleteUsername = Query.EQ("_id", username);
            var deletePassword = Query.EQ(Users.Password.Key, password);
            var deleteUser = Query.And(deleteUsername, deletePassword);
            UsersCollection.Collection.Remove(deleteUser);
        }

        public bool ValidateUser(string username, string password)
        {
            var usernameExists = Query.EQ("_id", username);
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
            var updateUser = Query.EQ("_id", user._id.ToString());
            var update = Update.Set("_id", user._id.ToString()).
                                Set(Users.Password.Key, user.Password).
                                Set(Users.Email.Key, user.Email);

            UsersCollection.Collection.Update(updateUser, update);
        }

        public List<User> GetAllUsers()
        {
            return UsersCollection.Collection.FindAllAs<User>().ToList();
        }
    }
}