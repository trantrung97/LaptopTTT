using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        QLLaptopEntities db = new QLLaptopEntities();
        // GET: Admin/Contact
        public ActionResult contact()
        {
            return View(db.ContactUS.ToList());
        }
    }
}