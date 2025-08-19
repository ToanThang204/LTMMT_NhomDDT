using System.ComponentModel.DataAnnotations;

namespace DoAnMangMayTinh.Models
{
    public class MonThi
    {
        [Key]
        public int ID_Mon { get; set; }
        public string TenMon { get; set; }
    }
}
