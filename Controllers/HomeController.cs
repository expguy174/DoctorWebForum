using DoctorWebForum.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoctorWebForum.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new WebForumEntities())
                {
                    int numberOfLoggedInUsers = MvcApplication.LoggedInUsersCount;
                    Session["NumberOfLoggedInUsers"] = numberOfLoggedInUsers;
                    int totalUserIds = db.users.Count();
                    Session["TotalMember"] = totalUserIds.ToString();
                    var posts = (from p in db.posts
                                 join pr in db.profiles on p.users_id equals pr.users_id
                                 select new homeViewModel
                                 {
                                     id = p.id,
                                     title = p.title,
                                     description = p.description,
                                     content = p.content,
                                     created_at = p.created_at,
                                     name = pr.name
                                 })
                                 .OrderByDescending(p => p.created_at)
                                 .Take(3)
                                 .ToList();
                    return View(posts);
                }
            }
            catch (DbException ex)
            {
                ViewBag.Error = "Database error occurred: " + ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
            }
            return View();
        }

        public ActionResult Main()
        {
            try
            {
                using (var db = new WebForumEntities())
                {
                    int numberOfLoggedInUsers = MvcApplication.LoggedInUsersCount;
                    Session["NumberOfLoggedInUsers"] = numberOfLoggedInUsers;
                    int totalUserIds = db.users.Count();
                    Session["TotalMember"] = totalUserIds.ToString();
                    var posts = (from p in db.posts
                                 join pr in db.profiles on p.users_id equals pr.users_id
                                 select new homeViewModel
                                 {
                                     id = p.id,
                                     title = p.title,
                                     description = p.description,
                                     content = p.content,
                                     created_at = p.created_at,
                                     name = pr.name
                                 })
                                 .OrderByDescending(p => p.created_at)
                                 .Take(3)
                                 .ToList();
                    return View(posts);
                }
            }
            catch (DbException ex)
            {
                ViewBag.Error = "Database error occurred: " + ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}