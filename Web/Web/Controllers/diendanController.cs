using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class diendanController : Controller
    {
        QLLaptopEntities db = new QLLaptopEntities();
        // GET: DienDan
        public ActionResult traodoi()
        {

            return View(db.BINHLUANs.ToList());
        }
        [HttpPost]
        public ActionResult traodoi(FormCollection frmbl, BINHLUAN bl)
        {
           
            bl.TenKH = frmbl["tenkh"];
            bl.TG = DateTime.Now;
            bl.NoiDung = frmbl["binhluan"];
           
            db.BINHLUANs.Add(bl);
            db.SaveChanges();
          
            return RedirectToAction("traodoi", "diendan");
        }
    }
}