using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class LichThi
    {
        [Key]
        public int ID_Lich { get; set; }

        [Required]
        [ForeignKey("KyThi")]
        public int ID_KyThi { get; set; }
        

        [Required]
        [ForeignKey("MonThi")]
        public int ID_Mon { get; set; }
        

        [Required]
        [ForeignKey("PhongThi")]
        public int ID_Phong { get; set; }
        

        public DateTime NgayThi { get; set; }
        public TimeSpan GioThi { get; set; }

        public KyThi KyThi { get; set; }
        public MonThi MonThi { get; set; }
        public PhongThi PhongThi { get; set; }

        public ICollection<DangKy> DangKys { get; set; }
        public ICollection<PhanCong> PhanCongs { get; set; }
    }
}
