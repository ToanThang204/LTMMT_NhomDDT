using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnMangMayTinh.Controllers
{

    public class SinhVienController : Controller
    {
        private readonly AppDbContext _context;

        public SinhVienController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sinhViens = await _context.SinhViens.Include(s => s.Lop).ToListAsync();
            return View(sinhViens);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var sv = await _context.SinhViens.Include(s => s.Lop).FirstOrDefaultAsync(s => s.ID_SV == id);
            if (sv == null) return NotFound();
            return View(sv);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.TenLop = new SelectList(await _context.Lops.ToListAsync(), "MaLop", "TenLop");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoTen,NgaySinh,MaLop")] SinhVien sv)
        {
            Console.WriteLine($"HoTen: {sv.HoTen}, NgaySinh: {sv.NgaySinh}, MaLop: {sv.MaLop}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState không hợp lệ:");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($" - {key}: {error.ErrorMessage}");
                    }
                }

                ViewBag.TenLop = new SelectList(await _context.Lops.ToListAsync(), "MaLop", "TenLop", sv.MaLop);
                return View(sv);
            }
            try
            {
                _context.SinhViens.Add(sv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm sinh viên: " + ex.Message);
                ModelState.AddModelError("", "Lỗi khi thêm sinh viên.");
                ViewBag.TenLop = new SelectList(await _context.Lops.ToListAsync(), "MaLop", "TenLop", sv.MaLop);
                return View(sv);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv == null) return NotFound();
            ViewBag.MaLop = new SelectList(await _context.Lops.ToListAsync(), "MaLop", "TenLop", sv.MaLop);
            return View(sv);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SinhVien sv)
        {
            if (id != sv.ID_SV) return NotFound();
            if (!ModelState.IsValid)
            {
                // 👇 Nạp lại ViewBag nếu có lỗi validation
                ViewBag.MaLop = new SelectList(await _context.Lops.ToListAsync(), "MaLop", "TenLop", sv.MaLop);
                return View(sv);
            }
            try
            {
                _context.Update(sv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SinhViens.Any(e => e.ID_SV == id)) return NotFound();
                else throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var sv = await _context.SinhViens.Include(s => s.Lop).FirstOrDefaultAsync(m => m.ID_SV == id);
            if (sv == null) return NotFound();
            return View(sv);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sv = await _context.SinhViens.FindAsync(id);
            if (sv != null)
            {
                _context.SinhViens.Remove(sv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}