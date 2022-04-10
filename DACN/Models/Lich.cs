namespace DACN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lich")]
    public partial class Lich
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lich()
        {
            LichHens = new HashSet<LichHen>();
        }

        [Key]
        public int MaLich { get; set; }

        public int? MaBacSi { get; set; }

        public DateTime? Ngay { get; set; }

        public int? TrangThai { get; set; }

        public virtual BacSi BacSi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichHen> LichHens { get; set; }
    }
}
