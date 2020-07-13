using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.Data;
namespace Web.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QLLaptopEntities db = new QLLaptopEntities();
        public List<CartItem> laygiohang()
        {
            List<CartItem> lstGioHang = Session["CartItem"] as List<CartItem>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<CartItem>();
                Session["CartItem"] = lstGioHang;
            }
            return lstGioHang;
        }
        [HttpPost]
        public ActionResult ThemGioHang(int iMaSP, int? SL)
        {
            List<CartItem> lstSP = laygiohang();
            CartItem SP = lstSP.Find(n => n.MaSPham == iMaSP);
            if (SP == null)
            {
                SP = new CartItem();
                SANPHAM tui = db.SANPHAMs.Single(n => n.MaSP == iMaSP);
                SP.MaSPham = iMaSP;
                SP.TenSPham = tui.TenSP;
                SP.AnhSP = tui.HinhMinhHoa;
                SP.GiaSP = double.Parse(tui.DonGia.ToString());
                if (SL == null)
                {
                    SP.SoLuong = 1;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                lstSP.Add(SP);

                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SL == null)
                {
                    SP.SoLuong++;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GioHang()
        {


            List<CartItem> lstgiohang = laygiohang();
            int TongSL = 0;
            double TongTien = 0;
            foreach (var item in lstgiohang)
            {
                TongSL += item.SoLuong;
                TongTien += item.TongTien;
            }
            Session["TongSL"] = TongSL.ToString();
            Session["TongTien"] = TongTien.ToString();

            return View(lstgiohang);
        }
        [HttpPost]
        public ActionResult XoaSP(int iMaSP)
        {
            List<CartItem> lstSP = laygiohang();
            CartItem SP = lstSP.Find(n => n.MaSPham == iMaSP);
            lstSP.Remove(SP);
            Session["GioHang"] = lstSP;
            return Json(lstSP, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CartItem> listSP = laygiohang();
            int TongSL = 0;
            double TongTien = 0;
            foreach (var item in listSP)
            {
                TongSL += item.SoLuong;
                TongTien += item.TongTien;
            }
            Session["TongSL"] = TongSL.ToString();
            Session["TongTien"] = TongTien.ToString();
            return View(listSP);

        }
        public ActionResult thanhtoanthanhcong()
        {

            return View();
        }
        [HttpPost]
        public ActionResult thanhtoanthanhcong(FormCollection frmTT, DON_DAT_HANG KH, CTDON_HANG CT)
        {
            KHACH_HANG kh = db.KHACH_HANG.Find(Session["KhachHang"]);
            KH.MaKH = Convert.ToInt32(kh.MaKH);
            KH.TenKH = frmTT["TenKH"];
            KH.Email = kh.Email;
            KH.DiaChi = frmTT["diachi"];
            KH.SDT = Convert.ToInt32(frmTT["SDT"]);
            KH.NgayDatHang = DateTime.Now;
            KH.NgayGiaoHang = DateTime.Now.AddDays(7);
            KH.TriGiaDH = Convert.ToInt32(frmTT["tien"]);
            db.DON_DAT_HANG.Add(KH);
            db.SaveChanges();
            List<CartItem> listSP = laygiohang();
            foreach (var item in listSP)
            {
                CTDON_HANG ctdh = new CTDON_HANG();
                ctdh.MaDH = KH.MaDH;
                ctdh.MaSP = Convert.ToInt32(item.MaSPham);
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = (decimal)item.GiaSP;
                
               
            SANPHAM sp = db.SANPHAMs.Single(n => n.MaSP == ctdh.MaSP);
                sp.SoLuong = sp.SoLuong - ctdh.SoLuong;
                db.SANPHAMs.Add(sp);
              db.CTDON_HANG.Add(ctdh);
                db.SaveChanges();
            }
            
            return RedirectToAction("thanhtoanthanhcong", "GioHang");
        }
        public ActionResult giohangcuatoi()
        {
            KHACH_HANG kh = db.KHACH_HANG.Find(Session["KhachHang"]);
            int makh = kh.MaKH;
            ViewBag.sklist = db.DON_DAT_HANG.Where(n => n.MaKH == makh).ToList();
            return View();
        }
    }
}
