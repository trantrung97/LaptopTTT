using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.Web.UI;

namespace Web.Controllers
{
    public class DKDNController : Controller
    {

        QLLaptopEntities db = new QLLaptopEntities();
        // GET: DKDN

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection frmDK, KHACH_HANG KH)
        {
            KH.TenKH = frmDK["HoTen"];
            KH.Email = frmDK["Email"];
            KH.DiaChi = frmDK["DiaChi"];
            KH.SDT = Convert.ToInt32(frmDK["SDT"]);
            KH.TenDN = frmDK["TenDN"];
            KH.MatKhau = GetMD5(frmDK["MatKhau"]);
            db.KHACH_HANG.Add(KH);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection frmDN)
        {
            string sTaiKhoan = frmDN["TaiKhoan"];
            string sMatKhau = GetMD5(frmDN["MK"]);
            KHACH_HANG KH = db.KHACH_HANG.SingleOrDefault(n => n.TenDN == sTaiKhoan && n.MatKhau == sMatKhau);
            if (KH != null)
            {
                Session["KhachHang"] = KH.MaKH;
                Session["TenKH"] = KH.TenKH;
                Session["ThongBao"] = "";
                return RedirectToAction("Index", "Home");
            }
            else
            {

                Session["ThongBao"] = "Mật Khẩu Hoặc Tài Khoản Không Chính Xác!";

            }
            return RedirectToAction("", "Home");
        }

        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            Session["TenKH"] = null;
            Session["ThongBao"] = "";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]

        public string GetMD5(string MD5)
        {
            string str = "";
            byte[] A = System.Text.Encoding.UTF8.GetBytes(MD5);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            A = md5.ComputeHash(A);
            foreach (Byte b in A)
            {
                str += b.ToString("X2");
            }
            return str;
        }

    }
}