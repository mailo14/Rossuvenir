using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KvotaWeb.Models;
using System.Web.Security;

namespace KvotaWeb.Controllers
{
        [Authorize]//(Roles = "Администратор, Менеджер")]
        public class UserController : Controller
        {
            private kvotaEntities db = new kvotaEntities();

            [HttpGet]
            public ActionResult Index()
            {
                var users = db.Rabotnik.ToList();//.Include(u => u.Role).ToList();//.Include(u => u.Department).Include(u => u.Role).ToList();
                return View(users);
            }
        }
    }
