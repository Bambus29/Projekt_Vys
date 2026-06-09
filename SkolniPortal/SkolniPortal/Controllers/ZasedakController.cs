using Microsoft.AspNetCore.Mvc;
using SkolniPortal.Migrations;
using SkolniPortal.Models;

namespace SkolniPortal.Controllers
{
    public class ZasedakController : Controller
    {
        private readonly SkolniPortalContext _db;
        public ZasedakController(SkolniPortalContext db) => _db = db;

        private bool IsUserLoggedIn() => HttpContext.Session.GetInt32("UserId") != null;
        private string GetUserKasta() => HttpContext.Session.GetString("UserKasta") ?? "";

        public IActionResult Index()
        {
            // Pokud není přihlášen, přesměruj na login
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");

            // Zobraz všechny zasedáky
            var zasedaky = _db.Zasedaky.ToList();
            return View(zasedaky);
        }

        public IActionResult ZasedakView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateZasedak(string forTrida, int pocetMist)
        {
            // Pouze učitelé mohou vytvářet
            if (!IsUserLoggedIn() || GetUserKasta() != "Učitel")
                return RedirectToAction("Index");

            // Validace
            if (string.IsNullOrWhiteSpace(forTrida) || pocetMist <= 0 || pocetMist > 50)
            {
                TempData["error"] = "Neplatné údaje. Počet míst musí být mezi 1 a 50.";
                return RedirectToAction("Index");
            }

            // Zkontroluj, že tato třída ještě nemá zasedák
            if (_db.Zasedaky.Any(z => z.forTrida == forTrida))
            {
                TempData["error"] = "Pro tuto třídu již zasedák existuje!";
                return RedirectToAction("Index");
            }

            // Vytvoř nový zasedák s prázdnými místy
            var z = new Zasedak
            {
                forTrida = forTrida,
                pocetMist = pocetMist,
                Mista = Enumerable.Repeat<string?>(null, pocetMist).ToList()!
            };

            _db.Zasedaky.Add(z);
            _db.SaveChanges();

            TempData["success"] = $"Zasedák pro třídu {forTrida} byl úspěšně vytvořen!";
            return RedirectToAction("Index");
        }

        public IActionResult Zasedak()
        {
            return View();
        }

    }
}
