using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using DataAccessLayer;

namespace BusinessLogic
{
    public class UsersBL
    {
        public UsersDAL dal
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
                _id = username,
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
    }
}
