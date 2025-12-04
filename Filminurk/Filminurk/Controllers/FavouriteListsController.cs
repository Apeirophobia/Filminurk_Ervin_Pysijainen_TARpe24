using System.Data;
using System.Security.Cryptography.X509Certificates;
using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

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
                    ListDeletedAt = (DateTime)x.ListDeletedAt,
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
            /*if (ModelState.IsValid == true)
            {*/
                List<Guid> tempParts = new();
                foreach (var stringID in userHasSelected)
                {
                    // lisame iga stringi kohta jäjrendis userhasselected teisendatud guidi
                    tempParts.Add(Guid.Parse(stringID));

                }
                // teeme uue DTO nimekirja jaoks
                var newListDto = new FavouriteListDTO()
                { 
                };

                newListDto.ListName = vm.ListName;
                newListDto.ListDescription = vm.ListDescription;
                newListDto.IsMovieOrActor = vm.IsMovieOrActor;
                newListDto.ListBelongsToUser = vm.ListBelongsToUser;
                newListDto.IsPrviate = vm.IsPrviate;
                newListDto.ListCreateAt = DateTime.UtcNow;
                newListDto.ListBelongsToUser = Guid.NewGuid().ToString();
                newListDto.ListModifiedAt = DateTime.UtcNow;
                newListDto.ListDeletedAt = vm.ListDeletedAt;
                
                // lisa filmid nimekirja, olemasolevate id-de põhiselt
                var listofmoviestoadd = new List<Movie>();
                foreach (var movieId in tempParts)
                {
                    var thismovie = _context.Movies.Where(tm => tm.ID == movieId).ToList().First();
                    listofmoviestoadd.Add((Movie)thismovie);
                }

                newListDto.ListOfMovies = listofmoviestoadd;


                // List<Guid> convertedIDs = new List<Guid>();
                //if (newListDto.ListOfMovies != null)
                //{
                //    convertedIDs = MovieToId(newListDto.ListOfMovies);
                //}

                var newList = await _FavouriteListsServices.Create(newListDto );   
                if (newList == null)
                {
                    return BadRequest();
                }

                return RedirectToAction("Index", vm);

            //}
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisuserid)
        {
            if (id == null || thisuserid == null)
            {
                return BadRequest();
                //TODO: return corresponding errorviews, id not found for list and user login error for userid
            }

            var thislist = _context.FavouriteLists
                .Where(l => l.FavouriteListID == id && l.ListBelongsToUser == thisuserid.ToString())
                .Select(
                stl => new FavouriteListUserDetailsViewModel
                {
                    FavouriteListID = stl.FavouriteListID,
                    ListBelongsToUser = stl.ListBelongsToUser,
                    IsMovieOrActor = stl.IsMovieOrActor,
                    ListName = stl.ListName,
                    ListDescription = stl.ListDescription,
                    IsPrviate = stl.IsPrviate,
                    ListOfMovies = stl.ListOfMovies,
                    IsReported = stl.IsReported,
                    /*Image = _context.FilesToDatabase
                    .Where(i => i.ListID == stl.FavouriteListID)
                    .Select(si => new FavouriteListIndexImageViewModel
                    {
                        ListID = si.ListID,
                        ImageID = si.ImageID,
                        ImageData = si.ImageData,
                        ImageTitle = si.ImageTitle,
                        Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(si.ImageData))
                    }).ToList().First()*/
                }).First();

            if (thislist == null)
            {
                return NotFound();
            }
            // add viewdata attribute here later to discernbetween user and admin
            return View("Details", thislist);
        }
        //[HttpGet]
        //public async Task<IActionResult> UserTogglePrivacy(Guid id, Guid thisuserid)
        //{
        //    if (id == null || thisuserid == null)
        //    {
        //        return BadRequest();
        //        //TODO: return corresponding errorviews, id not found for list and user login error for userid
        //    }

        //    var thislist = _context.FavouriteLists
        //        .Where(l => l.FavouriteListID == id && l.ListBelongsToUser == thisuserid.ToString())
        //        .Select(
        //        stl => new FavouriteListUserDetailsViewModel
        //        {
        //            FavouriteListID = stl.FavouriteListID,
        //            ListBelongsToUser = stl.ListBelongsToUser,
        //            IsMovieOrActor = stl.IsMovieOrActor,
        //            ListName = stl.ListName,
        //            ListDescription = stl.ListDescription,
        //            IsPrviate = stl.IsPrviate,
        //            ListOfMovies = stl.ListOfMovies,
        //            IsReported = stl.IsReported,
        //            /*Image = _context.FilesToDatabase
        //            .Where(i => i.ListID == stl.FavouriteListID)
        //            .Select(si => new FavouriteListIndexImageViewModel
        //            {
        //                ListID = si.ListID,
        //                ImageID = si.ImageID,
        //                ImageData = si.ImageData,
        //                ImageTitle = si.ImageTitle,
        //                Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(si.ImageData))
        //            }).ToList().First()*/
        //        }).First();

        //    if (thislist == null)
        //    {
        //        return NotFound();
        //    }
        //    // add viewdata attribute here later to discernbetween user and admin
        //    return View("UserTogglePrivacy", thislist);
        //}

        [HttpPost]
        public async Task<IActionResult> UserTogglePrivacy(Guid id)
        {
            FavouriteList thislist = await _FavouriteListsServices.DetailsAsync(id);

            FavouriteListDTO updateList = new FavouriteListDTO();

            updateList.FavouriteListID = thislist.FavouriteListID;
            updateList.ListBelongsToUser = thislist.ListBelongsToUser;
            updateList.IsMovieOrActor = thislist.IsMovieOrActor;
            updateList.ListName = thislist.ListName;
            updateList.ListDescription = thislist.ListDescription;
            updateList.IsPrviate = !thislist.IsPrviate;
            updateList.ListOfMovies = thislist.ListOfMovies;
            updateList.ListCreateAt = thislist.ListCreateAt;
            updateList.ListModifiedAt = DateTime.UtcNow;
            updateList.ListDeletedAt = thislist.ListDeletedAt;
            updateList.IsReported = thislist.IsReported;
            string UpdateServiceType = "Private";



            var result = await _FavouriteListsServices.Update(updateList, UpdateServiceType);
            
            if (result == null)
            {
                return BadRequest();
            }

            if (result.IsPrviate != !thislist.IsPrviate)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(Guid id)
        {
            var deletedList = await _FavouriteListsServices.DetailsAsync(id);

            deletedList.ListDeletedAt = DateTime.UtcNow;

            var dto = new FavouriteListDTO();

            dto.FavouriteListID = deletedList.FavouriteListID;
            dto.ListBelongsToUser = deletedList.ListBelongsToUser;
            dto.IsMovieOrActor = deletedList.IsMovieOrActor;
            dto.ListName = deletedList.ListName;
            dto.ListDescription = deletedList.ListDescription;
            dto.IsPrviate = deletedList.IsPrviate;
            dto.ListOfMovies = deletedList.ListOfMovies;
            dto.ListCreateAt = deletedList.ListCreateAt;
            dto.ListModifiedAt = DateTime.UtcNow;
            dto.ListDeletedAt = deletedList.ListDeletedAt;

            string UpdateServiceType = "Delete";


            var result = await _FavouriteListsServices.Update(dto, UpdateServiceType);

            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
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
