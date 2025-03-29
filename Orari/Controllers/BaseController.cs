using Microsoft.AspNetCore.Mvc;

namespace Orari.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
