using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorWebForum.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        private bool CheckLogin()
        {
            if (Session["login"] == null || Session["login"].ToString() != "true")
            {
                return false;
            }
            return true;
        }
    }
}