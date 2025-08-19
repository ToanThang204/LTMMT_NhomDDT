using DoAnMangMayTinh.Hubs;
using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace DoAnMangMayTinh.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<ThongBaoHub> _tbHub;
        public ThongBaoController(AppDbContext context, IHubContext<ThongBaoHub> tbHub)
        {
            _context = context;
            _tbHub = tbHub;
        }

        public async Task<IActionResult> Index() => View(await _context.ThongBaos.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ThongBaos.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public IActionResult Create() => View();

        [Authorize(Roles = "admin,giaovien")]
        [HttpPost]
        public async Task<IActionResult> Create(ThongBao model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                await _tbHub.Clients.All.SendAsync("ThongBaoMoi", new
                {
                    model.TieuDe,
                    model.NoiDung,
                    NgayDang = model.NgayDang.ToString("dd/MM/yyyy HH:mm")
                });
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ThongBaos.FindAsync(id);
            return View(item);
        }

        [Authorize(Roles = "admin,giaovien")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ThongBao model)
        {
            if (id != model.ID_TB) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                await _tbHub.Clients.All.SendAsync("CapNhatThongBao", new
                {
                    model.ID_TB,
                    model.TieuDe,
                    model.NoiDung,
                    NgayDang = model.NgayDang.ToString("dd/MM/yyyy HH:mm")
                });

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.ThongBaos.FindAsync(id);
            return View(item);
        }

        [Authorize(Roles = "admin,giaovien")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ThongBaos.FindAsync(id);
            _context.ThongBaos.Remove(item);
            await _context.SaveChangesAsync();
            await _tbHub.Clients.All.SendAsync("XoaThongBao", new
            {
                item.ID_TB
            });
            return RedirectToAction(nameof(Index));
        }
    }
}