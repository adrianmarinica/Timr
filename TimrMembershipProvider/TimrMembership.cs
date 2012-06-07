using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Security;
using BusinessLogic;
using Objects;

namespace TimrMembershipProvider
{
    public class TimrMembership : MembershipProvider
    {
        public override bool EnablePasswordRetrieval
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return bool.Parse(providerSettings.Parameters["enablePasswordRetrieval"]);
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return bool.Parse(providerSettings.Parameters["enablePasswordReset"]);
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return bool.Parse(providerSettings.Parameters["requiresQuestionAndAnswer"]);
            }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return int.Parse(providerSettings.Parameters["maxInvalidPasswordAttempts"]);
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return int.Parse(providerSettings.Parameters["passwordAttemptWindow"]);
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return bool.Parse(providerSettings.Parameters["requiresUniqueEmail"]);
            }
        }

        public string DefaultProvider
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                return membershipSection.DefaultProvider;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return
                    (MembershipPasswordFormat)
                    Enum.Parse(typeof (MembershipPasswordFormat), providerSettings.Parameters["requiresUniqueEmail"]);
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return int.Parse(providerSettings.Parameters["minRequiredPasswordLength"]);
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return int.Parse(providerSettings.Parameters["minRequiredNonAlphanumericCharacters"]);
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                var membershipSection = (MembershipSection) WebConfigurationManager.GetSection("system.web/membership");
                string defaultProvider = membershipSection.DefaultProvider;
                ProviderSettings providerSettings = membershipSection.Providers[defaultProvider];
                return providerSettings.Parameters["minRequiredNonAlphanumericCharacters"];
            }
        }

        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            var usersBL = new UsersBL();
            User user = usersBL.InsertUser(username, password, email, passwordQuestion, passwordAnswer, isApproved,
                                           providerUserKey, out status);
            return user != null ? user.ToMembershipUser(DefaultProvider) : null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            var usersBL = new UsersBL();
            return usersBL.ChangePasswordQuestionAndAnswer(username, password, newPasswordQuestion, newPasswordAnswer);
        }

        public override string GetPassword(string username, string answer)
        {
            var usersBL = new UsersBL();
            return usersBL.GetPassword(username, answer);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var usersBL = new UsersBL();
            return usersBL.ChangePassword(username, oldPassword, newPassword);
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var usersBL = new UsersBL();
            return (usersBL.ValidateUser(username, password, UserTypes.Student) ||
                usersBL.ValidateUser(username, password, UserTypes.Teacher) ||
                usersBL.ValidateUser(username, password, UserTypes.Faculty));
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            throw new NotImplementedException();
        }
    }
}