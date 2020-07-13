using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.Models;
using System.ComponentModel;
using System.Linq.Dynamic;

namespace Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QLLaptopEntities db = new QLLaptopEntities();
        // GET: Admin/IndexAdmin
        public ActionResult Index()
        {
            if (Session["TenAdmin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Login");
            }
            else
                return View();
        }
    }
}