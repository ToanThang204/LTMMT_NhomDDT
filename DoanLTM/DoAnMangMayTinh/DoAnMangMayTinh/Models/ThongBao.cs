using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class ThongBao
    {
        [Key]
        public int ID_TB { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDang { get; set; } = DateTime.Now;

    }
}
