using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class Lop
    {
        [Key]
        [Required]
        public string MaLop { get; set; }

        [Required]
        public string TenLop { get; set; }

        [ValidateNever]
        public ICollection<SinhVien> SinhViens { get; set; }
    }
}
