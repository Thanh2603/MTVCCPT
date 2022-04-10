using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DACN.ViewModel
{
    public class BSViewModel
    {
        [Key]
        public int MaBacSi { get; set; }

        [StringLength(250)]
        public string HinhAnh { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase avatarFile { get; set; }

        [StringLength(50)]
        public string TaiKhoan { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(200)]
        public string HoVaTen { get; set; }

        public int? MaChuyenKhoa { get; set; }

        [StringLength(10)]
        public string NgaySinh { get; set; }

        public int? MaGioiTinh { get; set; }

        public int? SDT { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public int? TrangThai { get; set; }
    }
}