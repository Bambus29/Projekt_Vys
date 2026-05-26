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
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string name, string password, string kasta, string trida)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["chyba"] = "Tato pole jsou povinná.";
                return View();
            }
            //if (password != passwordcheck)
            //{
            //    ViewData["chyba"] = "Hesla se neshodují.";
            //    return View();
            //}
            if (password.Length < 3)
            {
                ViewData["chyba"] = "Heslo musí být alespoň 3 znaky dlouhé.";
                return View();
            }
            if (_db.Users.Any(u => u.Name == name))
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
                 Name = name, 
                 Password = BCrypt.Net.BCrypt.HashPassword(password), 
                 Kasta = kasta, 
                 Trida = trida 
             };

            _db.Users.Add(user);
            _db.SaveChanges();
            ViewData["chyba"] = "Registrace úspěšná! Nyní se můžeš přihlásit.";
            return RedirectToAction("Login");
        }
    }

}
