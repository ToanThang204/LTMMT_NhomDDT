using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnMangMayTinh.Models
{
    public class DangKy
    {

        [Key]
        public int ID_DK { get; set; }

        [ForeignKey("SinhVien")]
        public int ID_SV { get; set; }

        [ValidateNever]
        public SinhVien SinhVien { get; set; }

        [ForeignKey("LichThi")]
        public int ID_Lich { get; set; }

        [ValidateNever]
        public LichThi LichThi { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}
