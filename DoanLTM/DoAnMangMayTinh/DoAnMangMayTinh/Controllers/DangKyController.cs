using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnMangMayTinh.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnMangMayTinh.Controllers
{
    public class DangKyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<DangKyHub> _dangKyHub;
        private readonly IHubContext<TongHopDangKyHub> _tongHopHub;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public DangKyController(AppDbContext context, IHubContext<DangKyHub> dangKyHub, IHubContext<TongHopDangKyHub> tongHopHub, IHubContext<NotificationHub> notificationHub)
        {
            _context = context;
            _dangKyHub = dangKyHub;
            _tongHopHub = tongHopHub;
            _notificationHub = notificationHub;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);

            if (taiKhoan?.ID_SV == null)
                return Unauthorized();

            var dangKys = await _context.DangKys
                .Include(d => d.LichThi).ThenInclude(l => l.MonThi)
                .Include(d => d.LichThi).ThenInclude(l => l.PhongThi)
                .Where(d => d.ID_SV == taiKhoan.ID_SV)
                .ToListAsync();
            var lichThiConChoRaw = await _context.LichThis.Include(l => l.MonThi).Include(l => l.PhongThi).Include(l => l.DangKys).Where(l => l.DangKys.Count < l.PhongThi.SoLuongChoNgoi).ToListAsync();
            var lichThiConCho = lichThiConChoRaw.Where(l => !dangKys.Any(dk => dk.ID_Lich == l.ID_Lich)).ToList();
            ViewBag.LichThiConCho = lichThiConCho;

            var lichSapThi = dangKys
                .Where(dk => dk.LichThi.NgayThi.Date == DateTime.Today.AddDays(1))
                .Select(dk => dk.LichThi.MonThi.TenMon)
                .ToList();

            if (lichSapThi.Any())
            {
                string message = "⚠️ Bạn có lịch thi ngày mai: " + string.Join(", ", lichSapThi);
                ViewBag.ThongBaoLichThi = message;

                await _notificationHub.Clients.User(taiKhoan.ID_SV.Value.ToString())
                    .SendAsync("hienThongBao", message);
            }

            return View(dangKys);
        }

        [Authorize(Roles = "sinhvien")]
        public async Task<IActionResult> DangKy(int idLich)
        {
            var username = User.Identity.Name;
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
            if (taiKhoan?.ID_SV == null) return Unauthorized();

            var exists = await _context.DangKys
                .AnyAsync(d => d.ID_SV == taiKhoan.ID_SV && d.ID_Lich == idLich);

            if (!exists)
            {
                var dk = new DangKy { ID_SV = taiKhoan.ID_SV.Value, ID_Lich = idLich };
                _context.DangKys.Add(dk);
                await _context.SaveChangesAsync();

                // Đếm lại số lượng mới sau khi lưu
                var soLuong = await _context.DangKys.CountAsync(dk => dk.ID_Lich == idLich);
                var totalDangKy = await _context.DangKys.CountAsync();

                await _dangKyHub.Clients.All.SendAsync("UpdateDangKy", new
                {
                    idLichThi = idLich,
                    soLuong,
                    totalDangKy,
                    message = "📢 Có sinh viên vừa đăng ký môn thi!"
                });
                await _tongHopHub.Clients.All.SendAsync("RefreshTongHopDangKy");


                TempData["Success"] = "Đăng ký thành công!";
            }
            else
            {
                TempData["Error"] = "Bạn đã đăng ký lịch thi này rồi!";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "sinhvien")]
        public async Task<IActionResult> XoaDangKy(int idLich)
        {
            var username = User.Identity.Name;
            var taiKhoan = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
            if (taiKhoan?.ID_SV == null) return Unauthorized();

            var dangKy = await _context.DangKys
                .FirstOrDefaultAsync(d => d.ID_SV == taiKhoan.ID_SV && d.ID_Lich == idLich);

            if (dangKy != null)
            {
                _context.DangKys.Remove(dangKy);
                await _context.SaveChangesAsync();

                var soLuong = await _context.DangKys.CountAsync(dk => dk.ID_Lich == idLich);
                var totalDangKy = await _context.DangKys.CountAsync();

                await _dangKyHub.Clients.All.SendAsync("UpdateDangKy", new
                {
                    idLichThi = idLich,
                    soLuong,
                    totalDangKy,
                    message = "Có sinh viên vừa hủy đăng ký môn thi!"
                });
                await _tongHopHub.Clients.All.SendAsync("RefreshTongHopDangKy");

                TempData["Success"] = "Hủy đăng ký thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> TongHopDangKy(int? monId, int? lichId, int? svId)
        {
            var query = _context.DangKys
                .Include(d => d.SinhVien).ThenInclude(s => s.Lop)
                .Include(d => d.LichThi).ThenInclude(l => l.MonThi)
                .Include(d => d.LichThi).ThenInclude(l => l.PhongThi)
                .AsQueryable();

            if (monId.HasValue)
                query = query.Where(d => d.LichThi.ID_Mon == monId);
            if (lichId.HasValue)
                query = query.Where(d => d.ID_Lich == lichId);
            if (svId.HasValue)
                query = query.Where(d => d.SinhVien.ID_SV == svId);

            ViewBag.MonThiList = new SelectList(_context.MonThis, "ID_Mon", "TenMon");
            ViewBag.LichThiList = new SelectList(_context.LichThis, "ID_Lich", "ID_Lich");
            ViewBag.SinhVienList = new SelectList(_context.SinhViens, "ID_SV", "HoTen");

            return View(await query.ToListAsync());
        }
    }
}