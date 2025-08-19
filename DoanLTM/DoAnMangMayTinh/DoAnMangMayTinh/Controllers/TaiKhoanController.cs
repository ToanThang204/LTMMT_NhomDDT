using DoAnMangMayTinh.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(TaiKhoan model)
    {
        if (ModelState.IsValid)
        {
            string hashedInput = HashPassword(model.MatKhau);

            var user = _context.TaiKhoans
                .FirstOrDefault(x => x.TenDangNhap == model.TenDangNhap &&
                                     x.MatKhau == hashedInput &&
                                     x.Loai.ToLower() == model.Loai.ToLower());

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.TenDangNhap),
                    new Claim(ClaimTypes.Role, user.Loai)
                };

                if (user.Loai.ToLower() == "sinhvien" && user.ID_SV.HasValue)
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID_SV.Value.ToString()));
                else if (user.Loai.ToLower() == "giaovien" && user.ID_GV.HasValue)
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, "gv-" + user.ID_GV.Value.ToString()));
                else if (user.Loai.ToLower() == "admin")
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, "admin"));

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return user.Loai.ToLower() switch
                {
                    "admin" => RedirectToAction("Index", "Home"),
                    "giaovien" => RedirectToAction("Index", "GiaoVien"),
                    "sinhvien" => RedirectToAction("Index", "SinhVien"),
                    _ => RedirectToAction("AccessDenied")
                };
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu sai.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }

    public IActionResult AccessDenied()
    {
        return Content("Bạn không có quyền truy cập chức năng này.");
    }
}