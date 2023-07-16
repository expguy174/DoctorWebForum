using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorWebForum;

namespace DoctorWebForum.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.user user)
        {
            try
            {
                using (var db = new DoctorWebForum.Models.WebForumEntities())
                {
                    var check = db.users.FirstOrDefault(v => v.username.Equals(user.username) && v.password.Equals(user.password));

                    if (check != null)
                    {
                        if (Session["user_id"] != null && Session["user_id"] as string != check.id.ToString())
                        {
                            // Xóa thông tin về tài khoản đang đăng nhập khỏi biến session
                            Session.Remove("user_id");
                            Session.Remove("username");
                            MvcApplication.LoggedInUsersCount--;
                        }

                        // Kiểm tra xem người dùng có đăng nhập với cùng một tài khoản hay không
                        if (Session["user_id"] == null || Session["user_id"] as string != check.id.ToString())
                        {
                            // Tạo một session mới cho người dùng đăng nhập với tài khoản khác
                            Session["user_id"] = check.id.ToString();
                            Session["username"] = check.username.ToString();
                            MvcApplication.LoggedInUsersCount++;
                        }
                        if (check.is_admin == true)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Notification = "Wrong username or password!";
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Something went wrong!";
            }
            return View();
        }

        public ActionResult Forgot()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.user user)
        {
            try
            {
                using (var db = new DoctorWebForum.Models.WebForumEntities())
                {
                    if(db.users.Any(u=>u.username == user.username))
                    {
                        ViewBag.Notification = "This account has already existed";
                        return View();
                    }
                    else
                    {
                        user.is_admin = false;
                        db.users.Add(user);
                        db.SaveChanges();
                        Session["user_id"] = user.id.ToString();
                        Session["username"] = user.username.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Something went wrong!";
            }
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["user_id"] != null)
            {
                MvcApplication.LoggedInUsersCount--;
                Session.Remove("user_id");
                Session.Remove("username");
                Session.Abandon();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}