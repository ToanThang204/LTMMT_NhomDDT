using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnMangMayTinh.Controllers
{
    [Authorize(Roles = "admin")]
    public class LopController : Controller
    {
        private readonly AppDbContext _context;

        public LopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Lops.ToListAsync());
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null) return NotFound();
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null) return NotFound();
            return View(lop);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lop lop)
        {
            Console.WriteLine(">>> POST Lop/Create được gọi");
            Console.WriteLine($"MaLop: {lop.MaLop}");
            Console.WriteLine($"TenLop: {lop.TenLop}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine(">>> ModelState INVALID");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"{modelState.Key}: {error.ErrorMessage}");
                    }
                }
                return View(lop);
            }

            try
            {
                _context.Add(lop);
                await _context.SaveChangesAsync();
                Console.WriteLine(">>> Lưu thành công vào DB!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu: " + ex.Message);
                ModelState.AddModelError("", "Không thể lưu vào CSDL: " + ex.Message);
            }

            return View(lop);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null) return NotFound();
            return View(lop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Lop lop)
        {
            if (id != lop.MaLop) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lops.Any(e => e.MaLop == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lop);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return NotFound();
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null) return NotFound();
            return View(lop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lop = await _context.Lops.FindAsync(id);
            _context.Lops.Remove(lop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}