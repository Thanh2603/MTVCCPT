namespace DACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichHen")]
    public partial class LichHen
    {
        [Key]
        public int MaLichHen { get; set; }

        public int? MaLich { get; set; }

        public int? MaBacSi { get; set; }

        public int? MaBenhNhan { get; set; }

        [StringLength(250)]
        public string NoiDung { get; set; }

        public int? TrangThai { get; set; }

        public virtual BacSi BacSi { get; set; }

        public virtual BenhNhan BenhNhan { get; set; }

        public virtual Lich Lich { get; set; }
    }
}
