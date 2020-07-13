using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.Reflection;
using System.Linq.Dynamic;
using PagedList;
using System.Net;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace Web.Areas.Admin.Controllers
{
    public class QuanlyController : Controller
    {
        private QLLaptopEntities db = new QLLaptopEntities();
        // GET: Admin/Quanly
        public ActionResult NSX()
        {
            if (Session["TenAdmin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Login");
            }
            else
            { 
                var f = from s in db.NHA_SAN_XUAT select s;
            ViewBag.sklist = db.NHA_SAN_XUAT.ToList();
            return View();}
        }
        [HttpPost]
        public ActionResult createNSX(FormCollection frmCreate, NHA_SAN_XUAT a)
        {

            a.TenNSX = frmCreate["ten"];
            a.DiaChi = frmCreate["diachi"];

            db.NHA_SAN_XUAT.Add(a);
            db.SaveChanges();
            return RedirectToAction("NSX", "Quanly");
        }
        public ActionResult Loai()
        {
            if (Session["TenAdmin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Login");
            }
            else
            { 
            var f = from s in db.PHAN_LOAI select s;
            ViewBag.sklist = db.PHAN_LOAI.ToList();
            return View();}
        }
        [HttpPost]
        public ActionResult createloai(FormCollection frmCreate, PHAN_LOAI a)
        {

            a.TenLoai = frmCreate["ten"];
            db.PHAN_LOAI.Add(a);
            db.SaveChanges();
            return RedirectToAction("Loai", "Quanly");
        }
        public ActionResult NhapKho()
        {
            
            if (Session["TenAdmin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Login");
            }
            else
            {
                List<SANPHAM> pub = db.SANPHAMs.ToList();
                SelectList ListPub = new SelectList(pub, "MaSP", "TenSP");
                ViewBag.tenhang = ListPub;
                /*List<NHA_SAN_XUAT> pub = db.NHA_SAN_XUAT.ToList();
                SelectList ListPub = new SelectList(pub, "MaNSX", "TenNSX");
                List<PHAN_LOAI> cate = db.PHAN_LOAI.ToList();
                SelectList ListCate = new SelectList(cate, "MaLoai", "TenLoai");
                ViewBag.NSX = ListCate;
                ViewBag.Loai = ListPub;*/
                var f = from s in db.NHAP_KHO select s;
                ViewBag.sklist = db.NHAP_KHO.ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult NhapKho(FormCollection frmCreate, NHAP_KHO a)
        {

            a.MaSP = Convert.ToInt32(frmCreate["masp"]);
            a.SoLuong = Convert.ToInt32(frmCreate["sl"]);
            SANPHAM sp = db.SANPHAMs.Single(n => n.MaSP == a.MaSP);
            sp.SoLuong = sp.SoLuong + a.SoLuong;
            a.ThoiGian = DateTime.Now;
           

            db.NHAP_KHO.Add(a);
            db.SaveChanges();
            return RedirectToAction("NhapKho", "Quanly");
        }
        public ActionResult tintuc()
        {
            if (Session["TenAdmin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Login");
            }
            else
              
            return View();
        
        }
        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }
        [HttpGet]
        public ActionResult tintuc(int? size, int? page, string sortProperty, string sortOrder, string searchString)
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

            var links = from l in db.TINTUCs select l;
            // 5. T?o thu?c tính s?p x?p m?c d?nh là "LinkID"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "STT";

            // 5. S?p x?p tang/gi?m b?ng phuong th?c OrderBy s? d?ng trong thu vi?n Dynamic LINQ
            if (sortOrder == "desc") links = links.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") links = links.OrderBy(sortProperty);
            else links = links.OrderBy("TenTT");

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.TenTT.Contains(searchString));
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
        public string ProcessUpload(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
            return "/Images/" + file.FileName;
        }
        [HttpPost]
        public ActionResult Createtintuc(FormCollection frmTao, TINTUC SP)
        {
            SP.TenTT = frmTao["TenTT"];
           
            SP.TomTat = frmTao["TT"];
            SP.href = frmTao["href"];
            SP.date = DateTime.Now;
            SP.HinhMinhHoa = frmTao["fileUpload"];
            db.TINTUCs.Add(SP);
            db.SaveChanges();
            return RedirectToAction("tintuc", "Quanly");
        }
        public ActionResult dondathang()
        {
          ViewBag.donhang = db.DON_DAT_HANG.Where(n => n.Dagiao == false).ToList();

            var f = from s in db.DON_DAT_HANG select s;
            ViewBag.sklist = db.DON_DAT_HANG.ToList();
            return View();

        }
       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DON_DAT_HANG dON_DAT_HANG = db.DON_DAT_HANG.Find(id);
            if (dON_DAT_HANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACH_HANG, "MaKH", "TenKH", dON_DAT_HANG.MaKH);
            return View(dON_DAT_HANG);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDH,MaKH,TenKH,DiaChi,SDT,Email,NgayDatHang,NgayGiaoHang,TriGiaDH,PTTT,HTGH,Dagiao")] DON_DAT_HANG dON_DAT_HANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dON_DAT_HANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("dondathang","Quanly");
            }
            ViewBag.MaKH = new SelectList(db.KHACH_HANG, "MaKH", "TenKH", dON_DAT_HANG.MaKH);
            return View(dON_DAT_HANG);
        }
        [HttpPost]
        public FileResult Export()
        {
            QLLaptopEntities entities = new QLLaptopEntities();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Mã Đơn Hàng"),
                                            new DataColumn("Tên Khách hàng"),
                                            new DataColumn("Ngày đặt hàng"),
                                            new DataColumn("giá đơn hàng"),
                                            new DataColumn("giao")
                                             });

            var customers = from customer in entities.DON_DAT_HANG.Take(10)
                            select customer;

            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.MaDH, customer.TenKH, customer.NgayDatHang, customer.TriGiaDH, customer.Dagiao);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }

}