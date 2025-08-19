using Microsoft.EntityFrameworkCore;

namespace DoAnMangMayTinh.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Lop> Lops { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<MonThi> MonThis { get; set; }
        public DbSet<GiaoVien> GiaoViens { get; set; }
        public DbSet<KyThi> KyThis { get; set; }
        public DbSet<PhongThi> PhongThis { get; set; }
        public DbSet<LichThi> LichThis { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<PhanCong> PhanCongs { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaiKhoan>()
                .Property(t => t.Loai)
                .HasConversion<string>();

            modelBuilder.Entity<PhanCong>()
                .Property(p => p.VaiTro)
                .HasConversion<string>();

            // Ràng buộc giá trị cho VaiTro
            modelBuilder.Entity<PhanCong>()
                .HasCheckConstraint("CK_VaiTro", "VaiTro IN ('Ra đề', 'Coi thi')");

            modelBuilder.Entity<TaiKhoan>()
                .HasCheckConstraint("CK_Loai", "Loai IN ('admin', 'sinhvien', 'giaovien')");
        }
    }

}
