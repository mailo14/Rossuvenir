using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace KvotaWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (!WebSecurity.Initialized)
            {
                /**/
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Rabotnik", "id", "login",
                    autoCreateTables:
                        true);
            }/**/
           /* var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            
                membership.CreateUserAndAccount("dev", "123");
            if (!roles.GetRolesForUser("Ефименко").Contains("Admin"))
            {
            //    roles.AddUsersToRoles(new[] { "devAdmin" }, new[] { "Admin" });
            }
            */

            //    if (!roles.RoleExists("Admin"))
            //    {
            //        roles.CreateRole("Admin");
            //        roles.CreateRole("Manager");
            //    }
            //    if (membership.GetUser("Admin", false) == null)
            //    {
            //        membership.CreateUserAndAccount("devAdmin", "Qwerty123");
            //    }
            //    if (!roles.GetRolesForUser("devAdmin").Contains("Admin"))
            //    {
            //        roles.AddUsersToRoles(new[] { "devAdmin" }, new[] { "Admin" });
            //    }
            //}



            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
