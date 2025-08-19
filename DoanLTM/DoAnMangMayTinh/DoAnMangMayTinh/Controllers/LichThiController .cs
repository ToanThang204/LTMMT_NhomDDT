using DoAnMangMayTinh.Hubs;
using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DoAnMangMayTinh.Hubs;
namespace DoAnMangMayTinh.Controllers
{
    [Authorize]
    public class LichThiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<LichThiHub> _hubContext;

        public LichThiController(AppDbContext context, IHubContext<LichThiHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.LichThis.Include(l => l.KyThi).Include(l => l.MonThi).Include(l => l.PhongThi).ToListAsync());

        }

        public async Task<IActionResult> Details(int id)
        {
            var lichThi = await _context.LichThis
        .Include(l => l.MonThi)
        .Include(l => l.PhongThi)
        .FirstOrDefaultAsync(l => l.ID_Lich == id);

            if (lichThi == null) return NotFound();

            var dangKyList = await _context.DangKys
                .Include(dk => dk.SinhVien)
                .Where(dk => dk.ID_Lich == id)
                .ToListAsync();

            ViewBag.DangKyList = dangKyList;
            return View(lichThi);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new LichThi());
        }
        
        [Authorize(Roles = "admin,giaovien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_KyThi,ID_Mon,ID_Phong,NgayThi,GioThi")] LichThi lichThi)
        {
            var keysToRemove = new[] { "KyThi", "MonThi", "PhongThi", "DangKys", "PhanCongs" };
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }
            bool isDuplicated = await _context.LichThis.AnyAsync(l =>
                l.NgayThi.Date == lichThi.NgayThi.Date &&
                l.GioThi == lichThi.GioThi &&
                (l.ID_Phong == lichThi.ID_Phong || l.ID_Mon == lichThi.ID_Mon)
            );

            if (isDuplicated)
            {
                ModelState.AddModelError("", "Lịch thi bị trùng phòng hoặc trùng môn vào cùng thời điểm!");
                LoadDropdowns();
                return View(lichThi);
            }

            if (ModelState.IsValid)
            {
                _context.Add(lichThi);
                await _context.SaveChangesAsync();
                var mon = await _context.MonThis.FindAsync(lichThi.ID_Mon);
                var message = $"📢 Lịch thi mới: {mon.TenMon} vào {lichThi.NgayThi:dd/MM/yyyy} lúc {lichThi.GioThi:hh\\:mm}";
                var phong = await _context.PhongThis.FindAsync(lichThi.ID_Phong);
                string trangThai;
                if (lichThi.NgayThi < DateTime.Today)
                    trangThai = "Đã thi";
                else if (lichThi.NgayThi == DateTime.Today)
                    trangThai = "Hôm nay";
                else
                    trangThai = "Sắp thi";
                await _hubContext.Clients.All.SendAsync("ReceiveLichThi", new
                {
                    id = lichThi.ID_Lich,
                    mon = mon.TenMon,
                    ngay = lichThi.NgayThi.ToString("dd/MM/yyyy"),
                    gio = lichThi.GioThi.ToString(@"hh\:mm"),
                    phong = phong?.TenPhong,
                    trangThai = trangThai,
                    message = message
                });
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(lichThi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.LichThis.FindAsync(id);
            if (item == null) return NotFound();
            LoadDropdowns();
            return View(item);
        }

        [Authorize(Roles = "admin,giaovien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Lich,ID_KyThi,ID_Mon,ID_Phong,NgayThi,GioThi")] LichThi model)
        {
            if (id != model.ID_Lich)
                return NotFound();

            var keysToRemove = new[] { "KyThi", "MonThi", "PhongThi", "DangKys", "PhanCongs" };
            foreach (var key in keysToRemove)
            {
                ModelState.Remove(key);
            }
            bool isDuplicated = await _context.LichThis.AnyAsync(l =>
                l.ID_Lich != model.ID_Lich &&
                l.NgayThi.Date == model.NgayThi.Date &&
                l.GioThi == model.GioThi &&
                (l.ID_Phong == model.ID_Phong || l.ID_Mon == model.ID_Mon)
            );

            if (isDuplicated)
            {
                ModelState.AddModelError("", "Lịch thi bị trùng phòng hoặc trùng môn vào cùng thời điểm!");
                LoadDropdowns();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var lichThi = await _context.LichThis.FindAsync(id);
                if (lichThi == null) return NotFound();

                lichThi.ID_KyThi = model.ID_KyThi;
                lichThi.ID_Mon = model.ID_Mon;
                lichThi.ID_Phong = model.ID_Phong;
                lichThi.NgayThi = model.NgayThi;
                lichThi.GioThi = model.GioThi;

                await _context.SaveChangesAsync();
                var mon = await _context.MonThis.FindAsync(model.ID_Mon);
                var message = $" Lịch thi cập nhật: {mon.TenMon} vào {model.NgayThi:dd/MM/yyyy} lúc {model.GioThi:hh\\:mm}";
                var phong = await _context.PhongThis.FindAsync(lichThi.ID_Phong);
                string trangThai;
                if (lichThi.NgayThi < DateTime.Today)
                    trangThai = "Đã thi";
                else if (lichThi.NgayThi == DateTime.Today)
                    trangThai = "Hôm nay";
                else
                    trangThai = "Sắp thi";

                await _hubContext.Clients.All.SendAsync("UpdateLichThi", new
                {
                    id = lichThi.ID_Lich,
                    mon = mon.TenMon,
                    ngay = model.NgayThi.ToString("dd/MM/yyyy"),
                    gio = model.GioThi.ToString(@"hh\:mm"),
                    phong = phong?.TenPhong,
                    trangThai = trangThai,
                    message = message
                });
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.LichThis.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [Authorize(Roles = "admin,giaovien")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.LichThis.FindAsync(id);
            if (item == null) return NotFound();
            var mon = await _context.MonThis.FindAsync(item.ID_Mon);
            _context.LichThis.Remove(item);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("DeleteLichThi", new
            {
                id = item.ID_Lich,
                message = $"Lịch thi môn {mon?.TenMon} ngày {item.NgayThi:dd/MM/yyyy} đã bị hủy!"
            });
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChiTietDangKy(int id)
        {
            var lichThi = await _context.LichThis
                .Include(l => l.KyThi)
                .Include(l => l.MonThi)
                .Include(l => l.PhongThi)
                .FirstOrDefaultAsync(l => l.ID_Lich == id);

            if (lichThi == null)
                return NotFound();

            var danhSachSV = await _context.DangKys
                .Where(d => d.ID_Lich == id)
                .Include(d => d.SinhVien)
                    .ThenInclude(s => s.Lop)
                .Select(d => new
                {
                    d.SinhVien.ID_SV,
                    d.SinhVien.HoTen,
                    d.SinhVien.NgaySinh,
                    Lop = d.SinhVien.Lop.TenLop
                })
                .ToListAsync();

            ViewBag.LichThi = lichThi;
            return View(danhSachSV);
        }

        private void LoadDropdowns()
        {
            ViewBag.ID_KyThi = new SelectList(_context.KyThis.ToList(), "ID_KyThi", "TenKy");
            ViewBag.ID_Mon = new SelectList(_context.MonThis.ToList(), "ID_Mon", "TenMon");
            ViewBag.ID_Phong = new SelectList(_context.PhongThis.ToList(), "ID_Phong", "TenPhong");
        }
    }
}