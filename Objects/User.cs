using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Objects
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public UserTypes UserType { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LastLockoutDate { get; set; }
        public List<UserRole> Roles { get; set; }

        public User()
        {
            Id = String.Empty;
            Password = String.Empty;
            Email = String.Empty;
            IsApproved = false;
            Name = string.Empty;
            PasswordAnswer = string.Empty;
            PasswordQuestion = string.Empty;
            Roles = new List<UserRole>();
        }

        public MembershipUser ToMembershipUser(string providerName)
        {
            return new MembershipUser(providerName, Id, Id, Email, PasswordQuestion, String.Empty, IsApproved, IsLockedOut, CreationDate, LastLoginDate, LastActivityDate, LastPasswordChangedDate, LastLockoutDate);
        }

        
    }
}
