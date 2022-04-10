namespace DACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChuyenKhoa")]
    public partial class ChuyenKhoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChuyenKhoa()
        {
            BacSis = new HashSet<BacSi>();
        }

        [Key]
        public int MaChuyenKhoa { get; set; }

        [StringLength(500)]
        public string TenChuyenKhoa { get; set; }

        [StringLength(250)]
        public string MoTa { get; set; }

        public int? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BacSi> BacSis { get; set; }
    }
}
