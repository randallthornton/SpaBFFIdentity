using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
