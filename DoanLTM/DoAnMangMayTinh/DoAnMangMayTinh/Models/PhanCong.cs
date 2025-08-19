using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnMangMayTinh.Models
{
    public class PhanCong
    {
        [Key]
        public int ID_PC { get; set; }

        [Required]
        [ForeignKey("GiaoVien")]
        public int ID_GV { get; set; }

        [ValidateNever]
        public GiaoVien GiaoVien { get; set; }

        [Required]
        [ForeignKey("LichThi")]
        public int ID_Lich { get; set; }

        [ValidateNever]
        public LichThi LichThi { get; set; }

        [Required]
        public string VaiTro { get; set; }
    }
}
