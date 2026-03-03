using Filminurk.Core.Domain;
using System.IO;
using Filminurk.Core.Dto.OMDB;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.Movies;
using Filminurk.Models.OMDB;
using Microsoft.AspNetCore.Mvc;
using Filminurk.Core.Dto;

namespace Filminurk.Controllers
{
    public class OMDBController : Controller
    {
        private readonly IOMDBServices _omdbServices;
        private readonly IMovieServices _movieServices;

        public OMDBController(IOMDBServices omdbServices, IMovieServices movieServices)
        {
            _omdbServices = omdbServices;
            _movieServices = movieServices;
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
                return RedirectToAction("Movie", "OMDB", new { movieName = vm.MovieTitle });
            }

            return View("Index");
        }

        [HttpGet]
        [ActionName("Movie")]
        public async Task<IActionResult> MovieGet(string movieName)
        {
            OMDBSearchResultDTO dto = new();
            dto.Title = movieName;

            _omdbServices.OMDBResult(dto);

            OMDBViewModel vm = new();
            vm.Title = dto.Title;
            vm.Year = dto.Year;
            vm.imdbID = dto.imdbID;
            vm.Rated = dto.Rated;
            vm.Released = dto.Released;
            vm.Runtime = dto.Runtime;
            vm.Genre = dto.Genre;
            vm.Director = dto.Director;
            return View(vm);
            return RedirectToAction("Movie", "OMDB");
        }

        [HttpPost]
        [ActionName("AddTheMovie")]
        public async Task<IActionResult> MoviePost(OMDBViewModel vm)
        {
            MoviesCreateUpdateViewModel newMovie = new MoviesCreateUpdateViewModel();

            newMovie.ID = new Guid();
            newMovie.Title = vm.Title;
            newMovie.FirstPublished = new DateOnly();
            newMovie.Director = vm.Director;
            newMovie.CurrentRating = 5;
            newMovie.Description = string.Empty;
            newMovie.EntryCreatedAt = DateTime.Now;
            newMovie.EntryModifiedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                
                return await RegisterMovie(newMovie);

            }
            Console.WriteLine("Modelstate invalid");
            
            return BadRequest();

            
            
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMovie(MoviesCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid == true)
            {

                var dto = new MoviesDTO()
                {
                    ID = vm.ID,
                    Title = vm.Title,
                    Description = vm.Description,
                    FirstPublished = vm.FirstPublished,
                    Director = vm.Director,
                    Actors = vm.Actors,
                    CurrentRating = vm.CurrentRating,
                    Vulgar = vm.Vulgar,
                    Genre = vm.Genre,
                    IsOnAdultSwim = vm.IsOnAdultSwim,
                    EntryCreatedAt = vm.EntryCreatedAt,
                    EntryModifiedAt = vm.EntryModifiedAt,

                    Files = vm.Files,
                    FileToApiDtos = vm.Images
                    .Select(x => new FileToApiDTO
                    {
                        ImageID = x.ImageID,
                        FilePath = x.FilePath,
                        MovieID = x.MovieID
                    }).ToArray()
                };

                var result = await _movieServices.Create(dto);
                if (result == null)
                {
                    return NotFound();
                }
                
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
