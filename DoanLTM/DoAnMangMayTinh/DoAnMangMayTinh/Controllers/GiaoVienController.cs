using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnMangMayTinh.Controllers
{
    public class GiaoVienController : Controller
    {
        private readonly AppDbContext _context;

        public GiaoVienController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin,giaovien")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GiaoViens.ToListAsync());
        }
        [Authorize(Roles = "admin,giaovien")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            if (giaoVien == null) return NotFound();
            return View(giaoVien);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiaoVien giaoVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaoVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Lỗi ở {entry.Key}: {error.ErrorMessage}");
                }
            }
            return View(giaoVien);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            if (giaoVien == null) return NotFound();
            return View(giaoVien);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GiaoVien giaoVien)
        {
            if (id != giaoVien.ID_GV) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giaoVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GiaoViens.Any(e => e.ID_GV == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(giaoVien);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            if (giaoVien == null) return NotFound();
            return View(giaoVien);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            _context.GiaoViens.Remove(giaoVien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}