using Microsoft.AspNetCore.Mvc;
using SkolniPortal.Data;
using SkolniPortal.Migrations;
using SkolniPortal.Models;

namespace SkolniPortal.Controllers
{
    public class ZasedakController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ZasedakController(ApplicationDbContext db) => _db = db;

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
        public IActionResult Zasedak() 
        {
            return View();
         }

        [HttpPost]
        public IActionResult CreateZasedak(string forTrida, int pocetMist)
        {
            // Pouze učitelé mohou vytvářet
            if (!IsUserLoggedIn() || GetUserKasta() != "Učitel")
                return RedirectToAction("Index");



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

        [HttpPost]
        public IActionResult SaveZasedak(int zasedakId, List<string> Mista)
        {
            // Pouze učitelé mohou upravovat
            if (!IsUserLoggedIn() || GetUserKasta() != "Učitel")
                return RedirectToAction("Index");

            // Najdi zasedák
            var zasedak = _db.Zasedaky.Find(zasedakId);
            if (zasedak == null)
            {
                TempData["error"] = "Zasedák nebyl nalezen!";
                return RedirectToAction("Index");
            }

            // Uprav seznam míst - vezmi jen tolik prvků, kolik je pocetMist
            zasedak.Mista = Mista?.Take(zasedak.pocetMist).ToList() ?? new List<string>();

            // Zajisti, aby měl správnou délku
            while (zasedak.Mista.Count < zasedak.pocetMist)
                zasedak.Mista.Add(null);

            _db.SaveChanges();

            TempData["success"] = $"Zasedák pro třídu {zasedak.forTrida} byl úspěšně uložen!";
            return RedirectToAction("Index");
        }

        public IActionResult ZasedakView()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "User");

            var zasedaky = _db.Zasedaky.ToList();
            return View(zasedaky);
        }

    }
}
