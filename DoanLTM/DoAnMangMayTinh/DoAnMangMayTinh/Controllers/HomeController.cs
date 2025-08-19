using System.Diagnostics;
using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnMangMayTinh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            AdminDashboardViewModel dashboard = null;
            if (User.IsInRole("admin"))
            {
                var today = DateTime.Today;
                dashboard = new AdminDashboardViewModel
                {
                    TotalSinhVien = _context.SinhViens.Count(),
                    TotalGiaoVien = _context.GiaoViens.Count(),
                    TotalLichThi = _context.LichThis.Count(),
                    TotalPhongThi = _context.PhongThis.Count(),
                    TotalThongBao = _context.ThongBaos.Count(),
                    TotalLop = _context.Lops.Count(),
                    TotalMonThi = _context.MonThis.Count(),
                    TotalDangKy = _context.DangKys.Count()
                };
            }
            return View(dashboard);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AdminDashboard()
        {
            var today = DateTime.Today;
            var model = new AdminDashboardViewModel
            {
                TotalSinhVien = _context.SinhViens.Count(),
                TotalGiaoVien = _context.GiaoViens.Count(),
                TotalLichThi = _context.LichThis.Count(),
                TotalPhongThi = _context.PhongThis.Count(),
                TotalThongBao = _context.ThongBaos.Count(),
                TotalLop = _context.Lops.Count(),
                TotalMonThi = _context.MonThis.Count(),
                TotalDangKy = _context.DangKys.Count()
            };
            return View(model);
        }
    }
}