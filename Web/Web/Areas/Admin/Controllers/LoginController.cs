using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class LoginController : Controller

    {
        QLLaptopEntities db = new QLLaptopEntities();
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(FormCollection frmDN)
        {
            //New dbConnect

            {

                string sTaiKhoan = frmDN["TaiKhoan"];
                string sMatKhau = frmDN["MK"];
                TKADMIN KH = db.TKADMINs.SingleOrDefault(n => n.TenDNAdmin == sTaiKhoan && n.MatKhauAdmin == sMatKhau);
                if (KH != null)
                {
                    Session["KhachHang"] = KH.MaAdmin;
                    Session["TenAdmin"] = KH.TenAdmin;
                    Session["ThongBao"] = "";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Session["ThongBao"] = "Mật Khẩu Hoặc Tài Khoản Không Chính Xác!";
                    return View("LoginAdmin");
                }
                
            }
        }
    }
}
