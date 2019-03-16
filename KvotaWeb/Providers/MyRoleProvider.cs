﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using KvotaWeb.Models;

namespace KvotaWeb.Providers
{
    public class MyRoleProvider   : RoleProvider
    {
        public override string[] GetRolesForUser(string login)
        {
            string[] role = new string[] { };
            using (kvotaEntities _db = new kvotaEntities())
            {
                try
                {
                    // Получаем пользователя
                    Rabotnik user = (from u in _db.Rabotnik
                                     where u.login == login
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                       // Role userRole = _db.Roles.Find(user.RoleId);

                        //if (userRole != null)
                        {
                            role = new string[] { user.uroven==1?"Администратор":"Менеджер" }; // userRole.Name };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }
            return role;
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            using (kvotaEntities _db = new kvotaEntities())
            {
                try
                {
                    // Получаем пользователя
                    Rabotnik user = (from u in _db.Rabotnik
                                     where u.login == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                       // Role userRole = _db.Roles.Find(user.RoleId);

                        //сравниваем
                        if (user.uroven == 1 && roleName== "Администратор" || user.uroven == 2 && roleName == "Менеджер") //userRole != null && userRole.Name == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
            }
            return outputResult;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}