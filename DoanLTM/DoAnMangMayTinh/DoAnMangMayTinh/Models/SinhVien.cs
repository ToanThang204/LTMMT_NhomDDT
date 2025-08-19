using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnMangMayTinh.Models
{
    public class SinhVien
    {
        [Key]
        public int ID_SV { get; set; }

        [Required]
        public string HoTen { get; set; }

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        [ForeignKey("Lop")]
        public string MaLop { get; set; }

        [ValidateNever]
        public Lop Lop { get; set; }

        [ValidateNever]
        public ICollection<DangKy> DangKys { get; set; }
    }

}
