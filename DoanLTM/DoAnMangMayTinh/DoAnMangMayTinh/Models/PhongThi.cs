using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class PhongThi
    {
        [Key]
        public int ID_Phong { get; set; }
        public string TenPhong { get; set; }
        public int SoLuongChoNgoi { get; set; }
    }
}