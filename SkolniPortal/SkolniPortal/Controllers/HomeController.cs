using Microsoft.AspNetCore.Mvc;
using SkolniPortal.Models;
using System.Diagnostics;

namespace SkolniPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

<<<<<<< HEAD
        public IActionResult Zasedak()
        {
            return View();
        }
        public IActionResult onas()
        {
            return View();
        }
=======
>>>>>>> 9a9ea50dd68f358aa76ec66d0f9124b94d60aab3


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
