using DACN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DACN.Controllers
{
    public class BenhAnController : Controller
    {
        private DACNDBContext db = new DACNDBContext();
        
        //DS Bệnh Án
        public ActionResult Index()
        {
            return View(db.BenhAns.ToList());
        }

        //Chi Tiết Bệnh Án
        public ActionResult Details(int? id)
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

        //Tạo Mới Bệnh Án
        public ActionResult Create()
        {
            ViewBag.MaLich = new SelectList(db.Liches.Where(p=>p.TrangThai == 2), "MaLich", "Ngay");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBenhAn,MaBacSi,MaBenhNhan,TenBenhNhan,NgayKham,NoiDung,ChuanDoan,DonThuoc,fileBenhAn,TrangThai")] BenhAn benhAn, int MaLich)
        {
            if (ModelState.IsValid)
            {
                db.BenhAns.Add(benhAn);
                Lich lich = db.Liches.Find(MaLich);
                lich.TrangThai = 3;
                benhAn.NgayKham = lich.Ngay;
                benhAn.TrangThai = 1;
                LichHen lh = db.LichHens.FirstOrDefault(p=>p.Lich.MaLich == MaLich);
                lh.TrangThai = 3;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(benhAn);
        }

        //Chỉnh Sửa Bệnh Án
        public ActionResult Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBenhAn,MaBacSi,MaBenhNhan,TenBenhNhan,NgayKham,NoiDung,ChuanDoan,DonThuoc,fileBenhAn,TrangThai")] BenhAn benhAn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(benhAn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(benhAn);
        }
    }
}