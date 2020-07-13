using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        QLLaptopEntities db = new QLLaptopEntities();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult GioiThieu()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ChonSP(int? IDNSX)
        {
            ViewBag.name = db.NHA_SAN_XUAT.SingleOrDefault(n => n.MaNSX == IDNSX).TenNSX;
            return View(db.SANPHAMs.Where(n => n.MaNSX == IDNSX).ToList());
        }
        public ActionResult LoaiSP(int? IDLoai)
        {
            return View(db.SANPHAMs.Where(n => n.MaLoai == IDLoai).ToList());
        }
        public ActionResult SPcaocap()
        {
            return View(db.SANPHAMs.Where(n => n.MaLoai == 2).Take(8).ToList());
        }
        public ActionResult banphimcaocap()
        {
            return View(db.SANPHAMs.Where(n => n.MaLoai == 4).Take(3).ToList());
        }
        public ActionResult cpu()
        {
            ViewBag.sklist = db.SANPHAMs.Where(n => n.MaLoai == 9).Take(2).ToList();
            return View(db.SANPHAMs.Where(n => n.MaLoai == 8).Take(2).ToList());
        }
        [HttpPost]
        public ActionResult contact(FormCollection frmCT, ContactU CT)
        {
            CT.Name = frmCT["name"];
            CT.Company = frmCT["organization"];
            CT.Email = frmCT["email"];
            CT.Phone = Convert.ToInt32(frmCT["phone"]);
            CT.Comments = frmCT["comment"];
            db.ContactUS.Add(CT);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Tintuc()
        {
            return View(db.TINTUCs.ToList());
        }
        public ActionResult ChiTietSP(int ID)
        {
            
          
            ViewBag.danhgia = db.DANHGIAs.Where(n => n.MaSP == ID).ToList();
            ViewBag.sklist = db.SANPHAMs.Where(n => n.MaLoai == 2).Take(4).ToList();
            return View(db.SANPHAMs.SingleOrDefault(n => n.MaSP == ID));
        }
        [HttpPost]
        public ActionResult danhgia(FormCollection frmdg, DANHGIA dg)
        {
            dg.MaSP = Convert.ToInt32(frmdg["masp"]);
            dg.TenKH = frmdg["tenkh"];
            dg.time = DateTime.Now;
            dg.comments = frmdg["binhluan"];
            dg.danhgia1 = Convert.ToInt32(frmdg["sao"]);
            
            db.DANHGIAs.Add(dg);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult SPtuongtu(int? IDLoai)
        {
            return View(db.SANPHAMs.Where(n => n.MaLoai == IDLoai).ToList());
        }

    }
}