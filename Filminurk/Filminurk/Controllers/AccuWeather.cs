using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class AccuWeather : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
