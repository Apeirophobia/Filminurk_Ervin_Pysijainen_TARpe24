using System.Data;
using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavouriteListsServices _FavouriteListsServices;
        // favouriteList services add later
        public FavouriteListsController(FilminurkTARpe24Context context, IFavouriteListsServices favouriteListsServices)
        {
            _context = context;
            _FavouriteListsServices = favouriteListsServices;
        }
        public IActionResult Index()
        {
            var resultingLists = _context.FavouriteLists
                .OrderByDescending(y => y.ListCreateAt)  // sorteeri nimekiri langevas jarjekorras kuupaeva jargi
                .Select(x => new FavouriteListsIndexViewModel
                {
                    FavouriteListID = x.FavouriteListID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    ListDescription = x.ListDescription,

                    ListCreateAt = x.ListCreateAt,
                    
                    Image =
                    (List<FavouriteListIndexImageViewModel>)_context.FilesToDatabase.Where(ml => ml.ListID == x.FavouriteListID)
                    .Select(li => new FavouriteListIndexImageViewModel()
                    {
                        ListID = li.ListID,
                        ImageID = li.ImageID,
                        ImageData = li.ImageData,
                        ImageTitle = li.ImageTitle,
                        Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(li.ImageData))
                    })
                    
                    // Image = x.Image.Select(img => new ImageViewModel
                    // {
                    //     ImageID = img.ImageID,
                    //     ExistingFilePath = img.ExistingFilePath
                    // }).ToList()  
                }
                );
            return View(resultingLists);
        }

        /*CREATE GET POST*/
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            /*TODO: Identify if user is an admin or registered user*/
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    Genre = mo.Genre
                })
                .ToList();
            ViewData["allmovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();

            FavouriteListsUserCreateViewModel result = new();
            return View("UserCreate", result);
        }


        [HttpPost]
        public async Task<IActionResult> UserCreate(FavouriteListsUserCreateViewModel vm, List<String> userHasSelected, 
            List<MoviesIndexViewModel> movies)
        {
            if (ModelState.IsValid == true)
            {
                List<Guid> tempParts = new();
                foreach (var stringID in userHasSelected)
                {
                    tempParts.Add(Guid.Parse(stringID));
                }

                var newListDto = new FavouriteListDTO()
                { 
                };

                newListDto.ListName = vm.ListName;
                newListDto.ListDescription = vm.ListDescription;
                newListDto.IsMovieOrActor = vm.IsMovieOrActor;
                newListDto.ListBelongsToUser = vm.ListBelongsToUser;
                newListDto.IsPrviate = vm.IsPrviate;
                newListDto.ListCreateAt = DateTime.UtcNow;
                newListDto.ListBelongsToUser = "00000000-0000-0000-0000000000000001";
                newListDto.ListModifiedAt = DateTime.UtcNow;
                newListDto.ListDeletedAt = (DateTime)vm.ListDeletedAt;

                List<Guid> convertedIDs = new List<Guid>();
                if (newListDto.ListOfMovies != null)
                {
                    convertedIDs = MovieToId(newListDto.ListOfMovies);
                }

                var newList = await _FavouriteListsServices.Create(newListDto, convertedIDs);
                if (newList != null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Index", vm);

            }
            return RedirectToAction("Index");
        }

        private List<Guid> MovieToId(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies)
            {
                result.Add(movie.ID);
            }
            return result;
        }
    }
}
