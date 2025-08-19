using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class KyThi
    {
        [Key]
        public int ID_KyThi { get; set; }

        [Required(ErrorMessage = "Tên kỳ thi không được để trống")]
        public string TenKy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NgayBatDau { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime NgayKetThuc { get; set; }
    }
}
