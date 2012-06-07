using System;
using System.Linq;
using System.Web.Security;
using DataAccessLayer;

namespace TimrRoleProvider
{
    public class TimrRole : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            var usersDAL = new UsersDAL();
            var user = usersDAL.GetUser(username);
            if (user.Roles != null)
                return user.Roles.Any(role => role.Name == roleName);
            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            var rolesDAL = new RolesDAL();
            return rolesDAL.GetRolesForUser(username);
        }

        public override void CreateRole(string roleName)
        {
            var rolesDal = new RolesDAL();
            rolesDal.CreateRole(roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            var rolesDal = new RolesDAL();
            return rolesDal.DeleteRole(roleName, throwOnPopulatedRole);
        }

        public override bool RoleExists(string roleName)
        {
            var rolesDal = new RolesDAL();
            return rolesDal.RoleExists(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var rolesDAL = new RolesDAL();
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    rolesDAL.AddUserToRole(username, roleName);
                }
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var rolesDAL = new RolesDAL();
            foreach (var username in usernames)
            {
                foreach (var roleName in roleNames)
                {
                    rolesDAL.RemoveUserFromRole(username, roleName);
                }
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var rolesDAL = new RolesDAL();
            return rolesDAL.GetUsersInRole(roleName);
        }

        public override string[] GetAllRoles()
        {
            var rolesDAL = new RolesDAL();
            return rolesDAL.GetAllRoles();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var rolesDAL = new RolesDAL();
            return rolesDAL.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string ApplicationName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}