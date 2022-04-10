using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DACN.Models
{
    public partial class DACNDBContext : DbContext
    {
        public DACNDBContext()
            : base("name=DACNDBContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<BacSi> BacSis { get; set; }
        public virtual DbSet<BenhAn> BenhAns { get; set; }
        public virtual DbSet<BenhNhan> BenhNhans { get; set; }
        public virtual DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }
        public virtual DbSet<GioiTinh> GioiTinhs { get; set; }
        public virtual DbSet<Lich> Liches { get; set; }
        public virtual DbSet<LichHen> LichHens { get; set; }
        public virtual DbSet<LienHe> LienHes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.TaiKhoan)
                .IsFixedLength();

            modelBuilder.Entity<Admin>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<BacSi>()
                .Property(e => e.TaiKhoan)
                .IsFixedLength();

            modelBuilder.Entity<BacSi>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<BacSi>()
                .Property(e => e.NgaySinh)
                .IsFixedLength();

            modelBuilder.Entity<BenhNhan>()
                .Property(e => e.TaiKhoan)
                .IsFixedLength();

            modelBuilder.Entity<BenhNhan>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<BenhNhan>()
                .Property(e => e.NgaySinh)
                .IsFixedLength();
        }
    }
}
