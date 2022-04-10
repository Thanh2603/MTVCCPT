using DACN.Models;
using DACN.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace DACN.Controllers
{
    public class AdminController : Controller
    {
        private DACNDBContext db = new DACNDBContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaiKhoan()
        {
            var taikhoans = db.Admins.ToList();
            return View(taikhoans);
        }

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
                var islogin = db.Admins.SingleOrDefault(x => x.TaiKhoan.Equals(tk) && x.MatKhau.Equals(mk));
                if (islogin != null)
                {
                    Session["userAdmin"] = islogin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.error = "Tài khoản hoặc mật khẩu không chính xác.";
                    SetAlert("Tài khoản hoặc mật khẩu không chính xác.", "error");
                    return View("Dangnhap");
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["userAdmin"] = null;
            return RedirectToAction("Dangnhap", "Admin");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTaiKhoan,TaiKhoan,MatKhau")] Admin taikhoans)
        {
            if (ModelState.IsValid)
            {
                var check = db.Admins.FirstOrDefault(s => s.TaiKhoan == taikhoans.TaiKhoan);
                if (check == null)
                {
                    db.Admins.Add(taikhoans);
                    db.SaveChanges();
                    SetAlert("Thêm thành công!", "success");
                    return RedirectToAction("TaiKhoan", "Admin");
                }
                else
                {
                    SetAlert("Thêm không thành công!", "error");
                    return RedirectToAction("TaiKhoan", "Admin");
                }
            }
            return View(taikhoans);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin taikhoans = db.Admins.Find(id);
            if (taikhoans == null)
            {
                return HttpNotFound();
            }
            return View(taikhoans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTaiKhoan,TaiKhoan,MatKhau")] Admin taikhoans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taikhoans).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Sửa thành công!", "success");
                return RedirectToAction("TaiKhoan");
            }
            SetAlert("Sửa không thành công!", "error");
            return View(taikhoans);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                Admin taikhoans = db.Admins.Find(id);
                db.Admins.Remove(taikhoans);
                db.SaveChanges();
                SetAlert("Xóa thành công!", "success");
                return RedirectToAction("TaiKhoan", "Admin");
            }
            SetAlert("Xóa không thành công!", "error");
            return RedirectToAction("TaiKhoan", "Admin");
        }

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