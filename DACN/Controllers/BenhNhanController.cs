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
    public class BenhNhanController : Controller
    {
        private DACNDBContext db = new DACNDBContext();

        //DS Bệnh Nhân
        public ActionResult TaiKhoanBN()
        {
            return View(db.BenhNhans.ToList());
        }

        //Chi Tiết Bệnh Nhân
        public ActionResult Details(int? id)
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
            return View(bn);
        }

        //Tạo tài khoản Bệnh Nhân
        public ActionResult Create()
        {
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BNViewModel model)
        {
            try
            {
                if (model.avatarFile != null && model.avatarFile.ContentLength > 0)
                {
                    string subPath = Server.MapPath("~/Content/HinhAnh/AvtBN/");
                    bool exists = System.IO.Directory.Exists(subPath);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(subPath);
                    }
                    string extension = Path.GetExtension(model.avatarFile.FileName);
                    model.HinhAnh = model.avatarFile.FileName;
                    model.avatarFile.SaveAs(Server.MapPath("~/Content/HinhAnh/AvtBN/") + model.HinhAnh);
                }
            }
            catch (Exception) { }
            BenhNhan bn = new BenhNhan
            {
                HinhAnh = model.HinhAnh,
                TaiKhoan = model.TaiKhoan,
                MatKhau = model.MatKhau,
                HoVaTen = model.HoVaTen,
                NgaySinh = model.NgaySinh,
                MaGioiTinh = model.MaGioiTinh,
                SDT = model.SDT,
                Email = model.Email,
                TrangThai = 1
            };
            var check = db.BenhNhans.FirstOrDefault(p => p.TaiKhoan == model.TaiKhoan);
            if (check == null)
            {
                db.BenhNhans.Add(bn);
                db.SaveChanges();
                SetAlert("Thêm tài khoản thành công", "success");
                return RedirectToAction("TaiKhoanBN");
            }
            else
            {
                SetAlert("Tên đăng nhập đã tồn tại", "error");
                return RedirectToAction("TaiKhoanBN");
            }
        }

        //Chỉnh Sửa Thông tin Bệnh Nhân
        public ActionResult Edit(int? id)
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
                Email = bn.Email,
                TrangThai = bn.TrangThai
            };
            ViewBag.MaGioiTinh = new SelectList(db.GioiTinhs, "MaGioiTinh", "TenGioiTinh", bn.MaGioiTinh);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BNViewModel model)
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
            bn.TrangThai = model.TrangThai;
            db.Entry(bn).State = EntityState.Modified;
            db.SaveChanges();
            SetAlert("Chỉnh sửa thành công", "success");
            return RedirectToAction("TaiKhoanBN");
        }

        //Xóa Tài Khoản Bệnh Nhân
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            BenhNhan bn = db.BenhNhans.Find(id);
            bn.TrangThai = 2;
            db.SaveChanges();
            return RedirectToAction("TaiKhoanBN");
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