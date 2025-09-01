using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AcademicEvents.Data;
using AcademicEvents.Models;
using AcademicEvents.ViewModels;
using AcademicEvents.Utils;

namespace AcademicEvents.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private const string CookieName = "AE.Remember";

        public AccountController(ApplicationDbContext db) => _db = db;

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (_db.Users.Any(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("", "E-mail já cadastrado.");
                return View(vm);
            }

            PasswordHasher.CreatePassword(vm.Email, vm.Password, out var hash, out var salt);
            var user = new User { Name = vm.Name, Email = vm.Email, PasswordHash = hash, Salt = salt };
            _db.Users.Add(user);
            _db.SaveChanges();

            SignIn(user, remember: false);
            return RedirectToAction("Index", "Events");
        }

        // GET: /Account/Login
        public IActionResult Login() => View(new LoginViewModel());

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = _db.Users.AsNoTracking().FirstOrDefault(u => u.Email == vm.Email);
            if (user == null || !PasswordHasher.Verify(vm.Email, vm.Password, user.Salt, user.PasswordHash))
            {
                ModelState.AddModelError("", "Credenciais inválidas.");
                return View(vm);
            }

            SignIn(user, vm.RememberMe);
            return RedirectToAction("Index", "Events");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(CookieName);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        private void SignIn(User user, bool remember)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "1" : "0");

            if (remember)
            {
                // Cookie simples para "lembrar" o usuário (não substituir autenticação real)
                Response.Cookies.Append(CookieName, $"{user.Id}|{user.Name}|{(user.IsAdmin ? 1 : 0)}",
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7), HttpOnly = true });
            }
        }

        // chamado pelo middleware (veja Startup) para reidratar sessão com cookie
        public static void RestoreSessionFromCookie(HttpContext ctx)
        {
            if (ctx.Session.GetInt32("UserId") != null) return;

            if (ctx.Request.Cookies.TryGetValue(CookieName, out var val))
            {
                var parts = val.Split('|');
                if (parts.Length == 3 && int.TryParse(parts[0], out int uid))
                {
                    ctx.Session.SetInt32("UserId", uid);
                    ctx.Session.SetString("UserName", parts[1]);
                    ctx.Session.SetString("IsAdmin", parts[2] == "1" ? "1" : "0");
                }
            }
        }
    }
}
