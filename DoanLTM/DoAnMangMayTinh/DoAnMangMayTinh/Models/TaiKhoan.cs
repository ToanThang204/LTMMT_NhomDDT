using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnMangMayTinh.Models
{
    public class TaiKhoan
    {
        [Key]
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Loai { get; set; }

        // Thêm khóa ngoại đến SinhVien
        public int? ID_SV { get; set; }
        [ForeignKey("ID_SV")]
        [ValidateNever]
        public SinhVien? SinhVien { get; set; }

        // Thêm khóa ngoại đến GiaoVien (giả sử có bảng GiaoVien)
        public int? ID_GV { get; set; }
        [ForeignKey("ID_GV")]
        [ValidateNever]
        public GiaoVien? GiaoVien { get; set; }
    }
}
