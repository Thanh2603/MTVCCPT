using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DACN.ViewModel
{
    public class TaoTaiKhoan
    {
        [StringLength(50)]
        public string TaiKhoan { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("MatKhau")]
        public string NhapLaiMatKhau { get; set; }

        [StringLength(250)]
        public string Ten { get; set; }
    }
}