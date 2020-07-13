using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using PagedList;
using System.Linq.Dynamic;

namespace Web.Areas.Admin.Controllers
{
    public class SANPHAMsController : Controller
    {
        private QLLaptopEntities db = new QLLaptopEntities();

        // GET: Admin/SANPHAMs
        public ActionResult SanPham()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SanPham(int? size, int? page, string sortProperty, string sortOrder, string searchString)
        {
            ViewBag.searchValue = searchString;
            ViewBag.sortProperty = sortProperty;
            ViewBag.page = page;

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            var links = from l in db.SANPHAMs select l;
            // 5. T?o thu?c tính s?p x?p m?c d?nh là "LinkID"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "MaSP";

            // 5. S?p x?p tang/gi?m b?ng phuong th?c OrderBy s? d?ng trong thu vi?n Dynamic LINQ
            if (sortOrder == "desc") links = links.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") links = links.OrderBy(sortProperty);
            else links = links.OrderBy("TenSP");

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.TenSP.Contains(searchString));
            }

            page = page ?? 1;


            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 6. Toán t? ?? trong C# mô t? n?u page khác null thì l?y giá tr? page, còn
            // n?u page = null thì l?y giá tr? 1 cho bi?n pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);

            // 6.2 L?y t?ng s? record chia cho kích thu?c d? bi?t bao nhiêu trang
            int checkTotal = (int)(links.ToList().Count / pageSize) + 1;
            // N?u trang vu?t qua t?ng s? trang thì thi?t l?p là 1 ho?c t?ng s? trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            return View(links.ToPagedList(pageNumber, pageSize));

        }

     

        // GET: Admin/SANPHAMs/Create
        public ActionResult Create()
        {
            List<NHA_SAN_XUAT> pub = db.NHA_SAN_XUAT.ToList();
            SelectList ListPub = new SelectList(pub, "MaNSX", "TenNSX");
            List<PHAN_LOAI> cate = db.PHAN_LOAI.ToList();
            SelectList ListCate = new SelectList(cate, "MaLoai", "TenLoai");
            ViewBag.NSX = ListCate;
            ViewBag.Loai = ListPub;
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
            return "/Images/" + file.FileName;
        }
        [HttpPost]

        public ActionResult Tao(FormCollection frmTao, SANPHAM SP)
        {
            SP.TenSP = frmTao["TenSP"];
            SP.MaNSX = Convert.ToInt32(frmTao["NHA_SAN_XUAT"]);
            SP.MaLoai = Convert.ToInt32(frmTao["PHAN_LOAI"]);
            SP.DonGia = Convert.ToDecimal(frmTao["DOnGia"]);
            SP.SoLuong = Convert.ToInt32(frmTao["SoLuong"]);
            SP.NoiDung = frmTao["NoiDung"];
            SP.ManHinh = frmTao["ManHinh"];
            SP.CPU = frmTao["CPU"];
            SP.RAM = frmTao["RAM"];
            SP.CauHinh = frmTao["CauHinh"];
            SP.HinhMinhHoa = frmTao["fileUpload"];
            db.SANPHAMs.Add(SP);
            db.SaveChanges();
            return RedirectToAction("SanPham", "SANPHAMs");
        }

    

        // GET: Admin/SANPHAMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUAT, "MaNSX", "TenNSX", sANPHAM.MaNSX);
            ViewBag.MaLoai = new SelectList(db.PHAN_LOAI, "MaLoai", "TenLoai", sANPHAM.MaLoai);
            return View(sANPHAM);
        }

        // POST: Admin/SANPHAMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,MaNSX,MaLoai,DonGia,HinhMinhHoa,SoLuong,NoiDung,ManHinh,CPU,RAM,CauHinh")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
               sANPHAM.NoiDung= sANPHAM.NoiDung.Replace("%3C/br%3E", "</br>");
                db.Entry(sANPHAM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SanPham","SANPHAMs");
            }
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUAT, "MaNSX", "TenNSX", sANPHAM.MaNSX);
            ViewBag.MaLoai = new SelectList(db.PHAN_LOAI, "MaLoai", "TenLoai", sANPHAM.MaLoai);
            return RedirectToAction("SanPham","SANPHAMs");
        }
       
        // GET: Admin/SANPHAMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: Admin/SANPHAMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sANPHAM);
            db.SaveChanges();
            return RedirectToAction("SanPham");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
