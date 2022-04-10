namespace DACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BenhAn")]
    public partial class BenhAn
    {
        [Key]
        public int MaBenhAn { get; set; }

        public int? MaBacSi { get; set; }

        public int? MaBenhNhan { get; set; }

        public DateTime? NgayKham { get; set; }

        public string NoiDung { get; set; }

        public string ChuanDoan { get; set; }

        public string DonThuoc { get; set; }

        public string fileBenhAn { get; set; }

        public int? TrangThai { get; set; }

        public virtual BacSi BacSi { get; set; }

        public virtual BenhNhan BenhNhan { get; set; }
    }
}
