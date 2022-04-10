using DACN.Models;
using DACN.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class HomeController : Controller
    {
        private DACNDBContext db = new DACNDBContext();

        //Đăng Nhập
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection Dangnhap)
        {
            if (ModelState.IsValid)
            {
                string tk = Dangnhap["TaiKhoan"].ToString();
                string mk = Dangnhap["MatKhau"].ToString();
                var islogin = db.BenhNhans.SingleOrDefault(x => x.TaiKhoan.Equals(tk) && x.MatKhau.Equals(mk) && x.TrangThai == 1);
                if (islogin != null)
                {
                    Session["userAdmin"] = islogin;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SetAlert("Đăng Nhập thất bại", "error");
                    return View("Dangnhap");
                }
            }
            return View();
        }

        //Đăng Ký Tài Khoản Bệnh Nhân
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(TaoTaiKhoan TaiKhoanMoi)
        {
            if (ModelState.IsValid)
            {
                var check = db.BenhNhans.FirstOrDefault(s => s.TaiKhoan == TaiKhoanMoi.TaiKhoan);
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    BenhNhan p = new BenhNhan
                    {
                        HoVaTen = TaiKhoanMoi.Ten,
                        TaiKhoan = TaiKhoanMoi.TaiKhoan,
                        MatKhau = TaiKhoanMoi.MatKhau
                    };
                    db.BenhNhans.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap");
                }
                else
                {
                    ViewBag.error = "Tài Khoản đã tồn tại";
                    return View();
                }
            }
            return View();
        }

        //Đăng Xuất
        public ActionResult DangXuat()
        {
            Session["userAdmin"] = null;
            return RedirectToAction("Dangnhap", "Home");
        }

        //Trang Chủ
        public ActionResult Index()
        {
            return View();
        }

        //Liên Hệ
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe([Bind(Include = "MaLienHe,Ten,Email,ChuDe,NoiDung,TrangThai")] LienHe lienHe)
        {
            db.LienHes.Add(lienHe);
            lienHe.TrangThai = 0;
            db.SaveChanges();
            return RedirectToAction("LienHe");
        }

        //Trang Chủ
        public ActionResult TrangChu()
        {
            return View();
        }

        //DS bệnh Án
        public ActionResult BenhAn(int id)
        {
            var benhAn = db.BenhAns.Where(p => p.MaBenhNhan == id).ToList();
            return View(benhAn);
        }

        public ActionResult ChiTietBA(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenhAn benhAn = db.BenhAns.Find(id);
            if (benhAn == null)
            {
                return HttpNotFound();
            }
            return View(benhAn);
        }

        //Thông Tin Bệnh Nhân
        public ActionResult ThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenhNhan bn = db.BenhNhans.Find(id);
            if (bn == null)
            {
                return HttpNotFound();
            }
            BNViewModel model = new BNViewModel
            {
                MaBenhNhan = bn.MaBenhNhan,
                HinhAnh = bn.HinhAnh,
                TaiKhoan = bn.TaiKhoan,
                MatKhau = bn.MatKhau,
                HoVaTen = bn.HoVaTen,
                NgaySinh = bn.NgaySinh,
                MaGioiTinh = bn.MaGioiTinh,
                SDT = bn.SDT,
                Email = bn.Email
            };
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh", bn.MaGioiTinh);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTin(BNViewModel model)
        {
            BenhNhan bn = db.BenhNhans.Find(model.MaBenhNhan);
            if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
            {
                string subPath = Server.MapPath("~/Content/HinhAnh/AvtBN/");
                bool exists = System.IO.Directory.Exists(subPath);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(subPath);
                }
                model.HinhAnh = model.avatarFile.FileName;
                model.avatarFile.SaveAs(Server.MapPath("~/Content/HinhAnh/AvtBN/") + model.HinhAnh);
            }
            else
            {
                model.HinhAnh = bn.HinhAnh.ToString();
            }
            bn.MaBenhNhan = model.MaBenhNhan;
            bn.HinhAnh = model.HinhAnh;
            bn.TaiKhoan = model.TaiKhoan;
            bn.MatKhau = model.MatKhau;
            bn.HoVaTen = model.HoVaTen;
            bn.NgaySinh = model.NgaySinh;
            bn.MaGioiTinh = model.MaGioiTinh;
            bn.SDT = model.SDT;
            bn.Email = model.Email;
            db.Entry(bn).State = EntityState.Modified;
            db.SaveChanges();
            SetAlert("Cập nhật thông tin thành công", "success");
            return RedirectToAction("ThongTin", "Home");
        }

        //Lịch Hẹn
        public ActionResult LichHen(int id)
        {
            var lh = db.LichHens.Where(p => p.MaBenhNhan == id && p.TrangThai != 3).ToList();
            return View(lh);
        }

        //Đặt Lịch Hẹn
        public ActionResult DatLichHen(int id)
        {
            var dem = db.Liches.Where(p => p.MaBacSi == id && p.TrangThai == 1).Count();
            if (dem > 0)
            {
                ViewBag.MaLich = new SelectList(db.Liches.Where(p => p.MaBacSi == id && p.TrangThai == 1).OrderBy(p => p.Ngay), "MaLich", "Ngay");
                return View();
            }
            else
            {
                return RedirectToAction("HetLich", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatLichHen([Bind(Include = "MaLichHen,MaLich,MaBacSi,MaBenhNhan,NoiDung,TrangThai")] LichHen lichHen, int id)
        {
            if (ModelState.IsValid)
            {
                db.LichHens.Add(lichHen);
                lichHen.MaBacSi = id;
                lichHen.TrangThai = 2;
                Lich lich = db.Liches.Find(lichHen.MaLich);
                lich.TrangThai = 2;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLich = new SelectList(db.Liches.Where(s => s.MaBacSi == id), "MaLich", "Ngay", lichHen.MaLich);
            return View(lichHen);
        }

        //Hết lịch
        public ActionResult HetLich()
        {
            return View();
        }

        //DS Bác Sĩ
        public ActionResult BacSi()
        {
            return View(db.BacSis.ToList());
        }

        //Thông Báo
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}