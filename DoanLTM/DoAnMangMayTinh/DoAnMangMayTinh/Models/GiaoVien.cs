using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class GiaoVien
    {
        [Key]
        public int ID_GV { get; set; }

        [Required]
        public string HoTen { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime? NgaySinh { get; set; }

        [ValidateNever]
        public ICollection<PhanCong> PhanCongs { get; set; }
    }
}
