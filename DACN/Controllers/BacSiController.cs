using DACN.Models;
using DACN.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class BacSiController : Controller
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
            try
            {
                string tk = Dangnhap["TaiKhoan"].ToString();
                string mk = Dangnhap["MatKhau"].ToString();
                var islogin = db.BacSis.SingleOrDefault(x => x.TaiKhoan.Equals(tk) && x.MatKhau.Equals(mk) && x.TrangThai == 1);
                if (islogin != null)
                {
                    Session["userAdmin"] = islogin;
                    return RedirectToAction("Index", "BacSi");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mật khẩu không chính xác.", "error");
                    return View("Dangnhap");
                }
            }
            catch (Exception)
            {
                SetAlert("Lỗi", "error");
                return View("Dangnhap");
            }
        }

        //Đăng Xuất
        public ActionResult DangXuat()
        {
            Session["userAdmin"] = null;
            return RedirectToAction("Dangnhap", "BacSi");
        }

        //DS bác sĩ
        public ActionResult TaiKhoanBS()
        {
            return View(db.BacSis.ToList());
        }

        //Chi tiết
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacSi bs = db.BacSis.Find(id);
            if (bs == null)
            {
                return HttpNotFound();
            }
            return View(bs);
        }

        //Tạo mới
        public ActionResult Create()
        {
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh");
            ViewBag.MaChuyenKhoa = new SelectList(db.ChuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BSViewModel model)
        {
            try
            {
                if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
                {
                    string subPath = Server.MapPath("~/Content/HinhAnh/AvtBS/");
                    bool exists = System.IO.Directory.Exists(subPath);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(subPath);
                    }
                    string extension = Path.GetExtension(model.avatarFile.FileName);
                    model.HinhAnh = model.avatarFile.FileName;
                    model.avatarFile.SaveAs(Server.MapPath("~/Content/HinhAnh/AvtBS/") + model.HinhAnh);
                }
            }
            catch (Exception) { }
            BacSi bs = new BacSi
            {
                HinhAnh = model.HinhAnh,
                TaiKhoan = model.TaiKhoan,
                MatKhau = model.MatKhau,
                HoVaTen = model.HoVaTen,
                MaChuyenKhoa = model.MaChuyenKhoa,
                NgaySinh = model.NgaySinh,
                MaGioiTinh = model.MaGioiTinh,
                SDT = model.SDT,
                Email = model.Email,
                TrangThai = 1
            };
            db.BacSis.Add(bs);
            db.SaveChanges();
            return RedirectToAction("TaiKhoanBS");
        }

        //Chỉnh Sửa
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacSi bs = db.BacSis.Find(id);
            if (bs == null)
            {
                return HttpNotFound();
            }
            BSViewModel model = new BSViewModel
            {
                MaBacSi = bs.MaBacSi,
                HinhAnh = bs.HinhAnh,
                TaiKhoan = bs.TaiKhoan,
                MatKhau = bs.MatKhau,
                MaChuyenKhoa = bs.MaChuyenKhoa,
                HoVaTen = bs.HoVaTen,
                NgaySinh = bs.NgaySinh,
                MaGioiTinh = bs.MaGioiTinh,
                SDT = bs.SDT,
                Email = bs.Email,
                TrangThai = 1
            };
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh", bs.MaGioiTinh);
            ViewBag.MaChuyenKhoa = new SelectList(db.ChuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa", bs.MaChuyenKhoa);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BSViewModel model)
        {
            BacSi bs = db.BacSis.Find(model.MaBacSi);
            if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
            {
                string subPath = Server.MapPath("~/Content/HinhAnh/AvtBS/");
                bool exists = System.IO.Directory.Exists(subPath);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(subPath);
                }
                model.HinhAnh = model.avatarFile.FileName;
                model.avatarFile.SaveAs(Server.MapPath("~/Content/HinhAnh/AvtBS/") + model.HinhAnh);
            }
            else
            {
                model.HinhAnh = bs.HinhAnh.ToString();
            }
            bs.MaBacSi = model.MaBacSi;
            bs.HinhAnh = model.HinhAnh;
            bs.TaiKhoan = model.TaiKhoan;
            bs.MatKhau = model.MatKhau;
            bs.MaChuyenKhoa = model.MaChuyenKhoa;
            bs.HoVaTen = model.HoVaTen;
            bs.NgaySinh = model.NgaySinh;
            bs.MaGioiTinh = model.MaGioiTinh;
            bs.SDT = model.SDT;
            bs.Email = model.Email;
            db.Entry(bs).State = EntityState.Modified;
            db.SaveChanges();
            SetAlert("Chỉnh sửa thành công!","success");
            return RedirectToAction("TaiKhoanBS");
        }

        //Xóa
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            BacSi bs = db.BacSis.Find(id);
            bs.TrangThai = 2;
            db.SaveChanges();
            return RedirectToAction("TaiKhoanBS");
        }

        //Trang chủ
        public ActionResult Index()
        {
            return View();
        }

        //Tạo Lịch Làm Việc
        public ActionResult Lich(int id)
        {
            var lich = db.Liches.Where(p => p.MaBacSi == id && p.TrangThai != 3).OrderBy(p => p.Ngay).ToList();
            return View(lich);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lich([Bind(Include = "MaLich,MaBacSi,Ngay,TrangThai")] Lich lich)
        {
            if (ModelState.IsValid)
            {
                db.Liches.Add(lich);
                lich.TrangThai = 1;
                db.SaveChanges();
                return RedirectToAction("Lich");
            }
            return View(lich);
        }

        //Xóa Lịch Làm Việc
        [HttpPost]
        public ActionResult XoaLich(int? id)
        {
            try
            {
                Lich lich = db.Liches.Find(id);
                lich.TrangThai = 3;
                db.SaveChanges();
                return RedirectToAction("Lich");
            }
            catch (Exception)
            {
                SetAlert("Lỗi", "error");
                return RedirectToAction("Lich");
            }
        }

        //DS Lịch Hẹn
        public ActionResult LichHenBS(int id)
        {
            var lh = db.LichHens.Where(p => p.MaBacSi == id && p.TrangThai != 3).ToList();
            return View(lh);
        }

        //Xác Nhận lịch Hẹn
        [HttpPost]
        public ActionResult LichHenBS(int? id, int id1)
        {
            try
            {
                Lich lich = db.Liches.Find(id);
                if (id1 == 1)
                {
                    LichHen lh = db.LichHens.Single(p=>p.MaLich == id);
                    lh.TrangThai = 1;
                    db.SaveChanges();
                    SetAlert("Chấp nhận thành công", "success");
                    return RedirectToAction("Lich");
                }else if (id1 == 0)
                {
                    lich.TrangThai = 1;
                    LichHen lh = db.LichHens.Find(lich.MaLich);
                    lh.TrangThai = 0;
                    db.SaveChanges();
                    SetAlert("Từ chối thành công", "success");
                    return RedirectToAction("Lich");
                }
                return RedirectToAction("Lich");
            }
            catch (Exception)
            {
                SetAlert("Lỗi", "error");
                return RedirectToAction("Lich");
            }
        }

        //Thông Tin Bác Sĩ
        public ActionResult ThongTin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacSi bs = db.BacSis.Find(id);
            if (bs == null)
            {
                return HttpNotFound();
            }
            BSViewModel model = new BSViewModel
            {
                MaBacSi = bs.MaBacSi,
                HinhAnh = bs.HinhAnh,
                TaiKhoan = bs.TaiKhoan,
                MatKhau = bs.MatKhau,
                HoVaTen = bs.HoVaTen,
                NgaySinh = bs.NgaySinh,
                MaGioiTinh = bs.MaGioiTinh,
                SDT = bs.SDT,
                Email = bs.Email,
                TrangThai = 1
            };
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh", bs.MaGioiTinh);
            ViewBag.MaChuyenKhoa = new SelectList(db.ChuyenKhoas, "MaChuyenKhoa", "TenChuyenKhoa", bs.MaChuyenKhoa);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTin(BSViewModel model)
        {
            BacSi bs = db.BacSis.Find(model.MaBacSi);
            if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
            {
                string subPath = Server.MapPath("~/Content/HinhAnh/AvtBS/");
                bool exists = System.IO.Directory.Exists(subPath);
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(subPath);
                }
                model.HinhAnh = model.avatarFile.FileName;
                model.avatarFile.SaveAs(Server.MapPath("~/Content/HinhAnh/AvtBS/") + model.HinhAnh);
            }
            else
            {
                model.HinhAnh = bs.HinhAnh.ToString();
            }
            bs.MaBacSi = model.MaBacSi;
            bs.HinhAnh = model.HinhAnh;
            bs.TaiKhoan = model.TaiKhoan;
            bs.MatKhau = model.MatKhau;
            bs.MaChuyenKhoa = model.MaChuyenKhoa;
            bs.HoVaTen = model.HoVaTen;
            bs.NgaySinh = model.NgaySinh;
            bs.MaGioiTinh = model.MaGioiTinh;
            bs.SDT = model.SDT;
            bs.Email = model.Email;
            bs.TrangThai = model.TrangThai;
            db.Entry(bs).State = EntityState.Modified;
            db.SaveChanges();
            SetAlert("Cập nhật thông tin thành công!","success");
            return RedirectToAction("ThongTin");
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