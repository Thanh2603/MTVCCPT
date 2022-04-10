using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DACN.ViewModel
{
    public class BNViewModel
    {
        [Key]
        public int MaBenhNhan { get; set; }

        [Required]
        [StringLength(250)]
        public string HinhAnh { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase avatarFile { get; set; }

        [Required]
        [StringLength(50)]
        public string TaiKhoan { get; set; }

        [Required]
        [StringLength(50)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(200)]
        public string HoVaTen { get; set; }

        [Required]
        [StringLength(10)]
        public string NgaySinh { get; set; }

        [Required]
        public int? MaGioiTinh { get; set; }

        [Required]
        public int? SDT { get; set; }

        [Required]
        [StringLength(500)]
        public string Email { get; set; }

        public int? TrangThai { get; set; }
    }
}