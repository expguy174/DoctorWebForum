using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorWebForum.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post()
        {
            return View();
        }

        public ActionResult Comment()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Edit_Post(int id)
        {
            return View();
        }

        public ActionResult Delete_Post(int id)
        {
            return View();
        }

        public ActionResult Edit_Comment(int id)
        {
            return View();
        }

        public ActionResult Delete_Comment(int id)
        {
            return View();
        }

        public ActionResult Create_User()
        {
            return View();
        }

        public ActionResult Edit_User(int id)
        {
            return View();
        }

        public ActionResult Delete_User(int id)
        {
            return View();
        }

        public ActionResult Create_Admin()
        {
            return View();
        }

        public ActionResult Edit_Admin(int id)
        {
            return View();
        }

        public ActionResult Delete_Admin(int id)
        {
            return View();
        }
    }
}