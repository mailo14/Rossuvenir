using AutoMapperAttributeMapping;
using KvotaWeb.Automapper;
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
           //var gg = Importer.Import();
            // Importer.Import(@"C:\Downloads\РАБОТА @\Калькулятор КВОТА 2.0\Rossuvenir\калькулятор 2020.04.01 - единый для импорта\калькулятор 2020.04.01 - шаблон.xlsx");//@"C:\Downloads\РАБОТА @\Калькулятор КВОТА 2.0\Rossuvenir\калькулятор 5 - блокноты, деколь\калькулятор 2020.04.01.xlsx");
            System.Web.Helpers.AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

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

            Mappings.RegisterMappings();
            ModelMetadataProviderConfig.RegisterModelMetadataProvider(); //This is what will bootstrap the bootstrap for the new 
        }
    }
}
