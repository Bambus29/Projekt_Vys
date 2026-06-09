using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkolniPortal.Models;
using BCrypt.Net;
using SkolniPortal.Migrations;

namespace SkolniPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly SkolniPortalContext _db;

        public UserController(SkolniPortalContext db)
        {
            _db = db;
        }

        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        private IActionResult RedirectIfNotLoggedIn()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login");
            return null;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["chyba"] = "Uveď uživatelské jméno a heslo.";
                return View();
            }

            // Normalize name for lookup (trim)
            var lookupName = name.Trim();

            // Use FirstOrDefault to avoid throwing if duplicates exist; the real fix is the DB unique index
            var user = _db.Users.FirstOrDefault(u => u.Name == lookupName);
            if (user == null)
            {
                ViewData["chyba"] = "Uživatel neexistuje.";
                return View();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewData["chyba"] = "Nesprávné heslo.";
                return View();
            }

            // Set session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserKasta", user.Kasta ?? string.Empty);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string password, string kasta, string trida, string passwordcheck)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["chyba"] = "Tato pole jsou povinná.";
                return View();
            }

            if (password != passwordcheck)
            {
                ViewData["chyba"] = "Hesla se neshodují.";
                return View();
            }

            if (password.Length < 3)
            {
                ViewData["chyba"] = "Heslo musí být alespoň 3 znaky dlouhé.";
                return View();
            }

            // Normalize name
            var normalized = name.Trim();

            if (_db.Users.Any(u => u.Name == normalized))
            {
                ViewData["chyba"] = "Tento login již existuje.";
                return View();
            }

            if (string.IsNullOrEmpty(kasta))
            {
                kasta = "Učitel";
            }
            else kasta = "Student";

            if (kasta == "Učitel")
            {
                trida = "N/A";
            }

            var user = new User
            {
                Name = normalized,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Kasta = kasta,
                Trida = trida
            };

            _db.Users.Add(user);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // Unique constraint violation or other DB error
                ViewData["chyba"] = "Tento login již existuje.";
                return View();
            }

            ViewData["chyba"] = "Registrace úspěšná! Nyní se můžeš přihlásit.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null)
                return redirect;

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
