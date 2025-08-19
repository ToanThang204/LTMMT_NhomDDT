using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DoAnMangMayTinh.Controllers
{
    [Authorize(Roles = "admin")]
    public class MonThiController : Controller
    {
        private readonly AppDbContext _context;
        public MonThiController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.MonThis.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var monThi = await _context.MonThis.FindAsync(id);
            if (monThi == null) return NotFound();
            return View(monThi);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonThi monThi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monThi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monThi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var monThi = await _context.MonThis.FindAsync(id);
            if (monThi == null) return NotFound();
            return View(monThi);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonThi monThi)
        {
            if (id != monThi.ID_Mon) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(monThi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monThi);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var monThi = await _context.MonThis.FindAsync(id);
            if (monThi == null) return NotFound();
            return View(monThi);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monThi = await _context.MonThis.FindAsync(id);
            if (monThi != null) _context.MonThis.Remove(monThi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
