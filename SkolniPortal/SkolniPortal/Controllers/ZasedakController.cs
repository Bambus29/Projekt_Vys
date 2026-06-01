using Microsoft.AspNetCore.Mvc;
using SkolniPortal.Migrations;
using SkolniPortal.Models;

namespace SkolniPortal.Controllers
{
    public class ZasedakController : Controller
    {
        private readonly SkolniPortalContext _db;
        public ZasedakController(SkolniPortalContext db)
        {
            _db = db;
        }
        [HttpPost]
        public IActionResult CreateZasedak(string forTrida, int pocetMist)
        {
            var z = new Zasedak
            {
                forTrida = forTrida,
                pocetMist = pocetMist,
                Mista = Enumerable.Repeat<string?>(null, pocetMist).ToList()!
            };

            _db.Zasedaky.Add(z);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
