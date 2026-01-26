using Filminurk.Core.Dto.OMDB;
using Filminurk.Models.OMDB;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OMDBController : Controller
    {
        
        public OMDBController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindMovie(OMDBSearchViewModel vm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Movie", "OMDB", new {movieName = vm.MovieTitle});
            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult Movie(string movieName)
        {
            OMDBSearchResultDTO dto = new();
            dto.Title = movieName;

            return View();
        }
    }
}
