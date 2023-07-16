using DoctorWebForum.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoctorWebForum.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(int? page)
        {
            try
            {
                using (var db = new WebForumEntities())
                {
                    int totalPosts = db.posts.Count();
                    int totalReplies = db.replies.Count();
                    int pageSize = 5; // số bản ghi trên mỗi trang
                    int pageNumber = (page ?? 1);
                    int totalRecords = db.posts.Count(); // tổng số bản ghi
                    int totalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
                    var topPosts = (from p in db.posts
                                    join pr in db.profiles on p.users_id equals pr.users_id
                                    join r in db.replies on p.id equals r.post_id into replies
                                    select new postViewModel
                                    {
                                        id = p.id,
                                        title = p.title,
                                        created_at = p.created_at,
                                        name = pr.name,
                                        reply_count = replies.Count()
                                    })
                                       .OrderByDescending(p => p.reply_count)
                                       .Take(5)
                                       .ToList();
                    var posts = (from p in db.posts
                                 join pr in db.profiles on p.users_id equals pr.users_id
                                 join l in db.locations on pr.location_id equals l.id
                                 join e in db.experiences on pr.experience_id equals e.id
                                 join s in db.specializations on pr.professional_id equals s.id
                                 join r in db.replies on p.id equals r.post_id into replies
                                 select new postViewModel
                                 {
                                     id = p.id,
                                     location_id = l.id,
                                     location_name = l.name,
                                     experience_id = e.id,
                                     experience_name = e.name,
                                     professional_id = s.id,
                                     professional_name = s.name,
                                     title = p.title,
                                     description = p.description,
                                     content = p.content,
                                     created_at = p.created_at,
                                     name = pr.name,
                                     reply_count = replies.Count()
                                 })
                                     .OrderByDescending(p => p.created_at)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = page;
                    ViewBag.TopPosts = topPosts;
                    ViewBag.TotalPosts = totalPosts;
                    ViewBag.TotalReplies = totalReplies;
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_PostList", posts);
                    }
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

        [HttpPost]
        public ActionResult Index(string searchTerm, int searchType, int? page)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm) && searchType == 0)
                {
                    return Content("");
                }

                using (var db = new WebForumEntities())
                {
                    int totalPosts = db.posts.Count();
                    int totalReplies = db.replies.Count();
                    int pageSize = 5; // số bản ghi trên mỗi trang
                    int pageNumber = (page ?? 1);
                    var topPosts = (from p in db.posts
                                    join pr in db.profiles on p.users_id equals pr.users_id
                                    join r in db.replies on p.id equals r.post_id into replies
                                    select new postViewModel
                                    {
                                        id = p.id,
                                        title = p.title,
                                        created_at = p.created_at,
                                        name = pr.name,
                                        reply_count = replies.Count()
                                    })
                                       .OrderByDescending(p => p.reply_count).Take(5).ToList();
                    List<DoctorWebForum.Models.postViewModel> results = new List<DoctorWebForum.Models.postViewModel>();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        switch (searchType)
                        {
                            case 1: // Tìm kiếm theo Location
                                results = (from p in db.posts
                                           join pr in db.profiles on p.users_id equals pr.users_id
                                           join l in db.locations on pr.location_id equals l.id
                                           join e in db.experiences on pr.experience_id equals e.id
                                           join s in db.specializations on pr.professional_id equals s.id
                                           where l.name.Contains(searchTerm) && searchType == 1
                                           select new postViewModel
                                           {
                                               location_name = l.name,
                                               experience_name = e.name,
                                               professional_name = s.name,
                                               title = p.title,
                                           }).ToList();
                                break;
                            case 2: // Tìm kiếm theo Professional
                                results = (from p in db.posts
                                           join pr in db.profiles on p.users_id equals pr.users_id
                                           join l in db.locations on pr.location_id equals l.id
                                           join e in db.experiences on pr.experience_id equals e.id
                                           join s in db.specializations on pr.professional_id equals s.id
                                           where s.name.Contains(searchTerm) && searchType == 2
                                           select new postViewModel
                                           {
                                               location_name = l.name,
                                               experience_name = e.name,
                                               professional_name = s.name,
                                               title = p.title,
                                           }).ToList();
                                break;
                            case 3: // Tìm kiếm theo Experience
                                results = (from p in db.posts
                                           join pr in db.profiles on p.users_id equals pr.users_id
                                           join l in db.locations on pr.location_id equals l.id
                                           join e in db.experiences on pr.experience_id equals e.id
                                           join s in db.specializations on pr.professional_id equals s.id
                                           where e.name.Contains(searchTerm) && searchType == 3
                                           select new postViewModel
                                           {
                                               location_name = l.name,
                                               experience_name = e.name,
                                               professional_name = s.name,
                                               title = p.title,
                                           }).ToList();
                                break;
                            default: // Tìm kiếm theo Topic
                                results = (from p in db.posts
                                           join pr in db.profiles on p.users_id equals pr.users_id
                                           join l in db.locations on pr.location_id equals l.id
                                           join e in db.experiences on pr.experience_id equals e.id
                                           join s in db.specializations on pr.professional_id equals s.id
                                           where p.title.Contains(searchTerm) && searchType == 0
                                           select new postViewModel
                                           {
                                               location_name = l.name,
                                               experience_name = e.name,
                                               professional_name = s.name,
                                               title = p.title,
                                           }).ToList();
                                break;
                        }
                    }
                    else if (searchType == 0)
                    {
                        ViewBag.Error = "Please enter a search term.";
                    }
                    else
                    {
                        results = (from p in db.posts
                                     join pr in db.profiles on p.users_id equals pr.users_id
                                     join l in db.locations on pr.location_id equals l.id
                                     join e in db.experiences on pr.experience_id equals e.id
                                     join s in db.specializations on pr.professional_id equals s.id
                                     join r in db.replies on p.id equals r.post_id into replies
                                     select new postViewModel
                                     {
                                         id = p.id,
                                         location_id = l.id,
                                         location_name = l.name,
                                         experience_id = e.id,
                                         experience_name = e.name,
                                         professional_id = s.id,
                                         professional_name = s.name,
                                         title = p.title,
                                         description = p.description,
                                         content = p.content,
                                         created_at = p.created_at,
                                         name = pr.name,
                                         reply_count = replies.Count()
                                     })
                                     .OrderByDescending(p => p.created_at)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();
                    }
                    // Phân trang
                    int totalRecords = results.Count(); // tổng số bản ghi
                    int totalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
                    var posts = results.OrderByDescending(p => p.created_at)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

                    ViewBag.TotalPosts = totalPosts;
                    ViewBag.TotalReplies = totalReplies;
                    ViewBag.TopPosts = topPosts;
                    ViewBag.SearchTerm = searchTerm;
                    ViewBag.SearchType = searchType;
                    ViewBag.PageNumber = pageNumber;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalPages = totalPages;
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_PostList", results);
                    }
                    return View(results);
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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Detail()
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