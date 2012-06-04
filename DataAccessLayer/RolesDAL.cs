﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Collections;
using MongoDB.Driver.Builders;
using Objects;

namespace DataAccessLayer
{
    public class RolesDAL
    {
        public void CreateRole(string roleName)
        {
            var role = new UserRole
                           {
                               Name = roleName,
                               UserType = (UserTypes) Enum.Parse(typeof (UserTypes), roleName)
                           };
            RolesCollection.Collection.Insert(role);
        }

        public string[] GetRolesForUser(string username)
        {
            var usersDAL = new UsersDAL();
            var user = usersDAL.GetUser(username);
            var list = new List<string>();

            if (user != null && user.Roles != null)
                list.AddRange(user.Roles.Select(role => role.Name));
            return list.ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            var usersDAL = new UsersDAL();
            var allUsers = usersDAL.GetAllUsers();
            var list = new List<string>();
            if (allUsers != null)
                foreach (var user in allUsers.Where(user => user.Roles != null))
                {
                    list.AddRange(from role in user.Roles where role.Name == roleName select user.Id);
                }
            return list.ToArray();
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (GetUsersInRole(roleName).Length > 0)
                return false;
            var query = Query.EQ("Name", roleName);
            RolesCollection.Collection.Remove(query);
            return true;
        }

        public bool RoleExists(string roleName)
        {
            var query = Query.EQ("Name", roleName);
            return RolesCollection.Collection.FindOneAs<UserRole>(query) != null;
        }

        public string[] GetAllRoles()
        {
            var roleList = RolesCollection.Collection.FindAllAs<UserRole>();
            return roleList.Select(userRole => userRole.Name).ToArray();
        }
    }
}