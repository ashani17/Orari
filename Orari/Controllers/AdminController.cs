using Microsoft.AspNetCore.Mvc;

namespace Orari.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
