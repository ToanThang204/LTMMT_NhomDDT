using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace DoAnMangMayTinh.Controllers
{
    public class PhanCongController : Controller
    {
        private readonly AppDbContext _context;

        public PhanCongController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var phanCongs = await _context.PhanCongs
                .Include(p => p.GiaoVien)
                .Include(p => p.LichThi)
                    .ThenInclude(l => l.MonThi)
                .ToListAsync();

            return View(phanCongs);
        }
        
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.PhanCongs
                .Include(p => p.GiaoVien)
                .Include(p => p.LichThi)
                    .ThenInclude(l => l.MonThi)
                .FirstOrDefaultAsync(p => p.ID_PC == id);

            if (item == null) return NotFound();

            return View(item);
        }

        public IActionResult Create()
        {
            ViewBag.ID_GV = new SelectList(_context.GiaoViens, "ID_GV", "HoTen");
            ViewBag.ID_Lich = new SelectList(
                _context.LichThis.Include(l => l.MonThi),
                "ID_Lich", "MonThi.TenMon"
            );
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(PhanCong model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ID_GV = new SelectList(_context.GiaoViens, "ID_GV", "HoTen", model.ID_GV);
            ViewBag.ID_Lich = new SelectList(
                _context.LichThis.Include(l => l.MonThi),
                "ID_Lich", "MonThi.TenMon", model.ID_Lich
            );
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.PhanCongs.FindAsync(id);
            if (item == null) return NotFound();

            ViewBag.ID_GV = new SelectList(_context.GiaoViens, "ID_GV", "HoTen", item.ID_GV);
            ViewBag.ID_Lich = new SelectList(
                _context.LichThis.Include(l => l.MonThi),
                "ID_Lich", "MonThi.TenMon", item.ID_Lich
            );

            return View(item);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PhanCong model)
        {
            if (id != model.ID_PC) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PhanCongs.Any(e => e.ID_PC == model.ID_PC))
                        return NotFound();
                    else
                        throw;
                }
            }
            ViewBag.ID_GV = new SelectList(_context.GiaoViens, "ID_GV", "HoTen", model.ID_GV);
            ViewBag.ID_Lich = new SelectList(
                _context.LichThis.Include(l => l.MonThi),
                "ID_Lich", "MonThi.TenMon", model.ID_Lich
            );

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.PhanCongs.FindAsync(id);
            return View(item);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.PhanCongs.FindAsync(id);
            _context.PhanCongs.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "giaovien")]
        public async Task<IActionResult> XemPhanCong()
        {
            var phanCong = await _context.PhanCongs
                .Include(p => p.GiaoVien)
                .Include(p => p.LichThi)
                    .ThenInclude(l => l.MonThi)
                .Include(p => p.LichThi)
                    .ThenInclude(l => l.PhongThi)
                .Include(p => p.LichThi)
                    .ThenInclude(l => l.KyThi)
                .ToListAsync();

            return View(phanCong);
        }
    }
}