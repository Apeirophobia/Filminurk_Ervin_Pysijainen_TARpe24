using Filminurk.ApplicationServices.Services;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        // favouriteList services add later
        private readonly FilesServices _filesServices;
        public FavouriteListsController(FilminurkTARpe24Context context, FilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
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
    }
}
