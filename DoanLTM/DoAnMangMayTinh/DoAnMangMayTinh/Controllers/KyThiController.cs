using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnMangMayTinh.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
namespace DoAnMangMayTinh.Controllers
{
    public class KyThiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        public KyThiController(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index() => View(await _context.KyThis.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var kyThi = await _context.KyThis.FindAsync(id);
            if (kyThi == null) return NotFound();
            return View(kyThi);
        }

        public IActionResult Create() => View();

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KyThi kyThi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kyThi);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage",
               $"[THÔNG BÁO] Kỳ thi mới được tạo: {kyThi.TenKy}");
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var err in errors)
            {
                Console.WriteLine(err.ErrorMessage);
            }
            return View(kyThi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var kyThi = await _context.KyThis.FindAsync(id);
            if (kyThi == null) return NotFound();
            return View(kyThi);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KyThi kyThi)
        {
            if (id != kyThi.ID_KyThi) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(kyThi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kyThi);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var kyThi = await _context.KyThis.FindAsync(id);
            if (kyThi == null) return NotFound();
            return View(kyThi);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kyThi = await _context.KyThis.FindAsync(id);
            if (kyThi != null) _context.KyThis.Remove(kyThi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}