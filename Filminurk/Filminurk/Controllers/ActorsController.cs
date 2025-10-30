using Filminurk.Data;
using Filminurk.Models.Actors;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        // private readonly IActorServices _actorServices;
        public ActorsController
            (
            FilminurkTARpe24Context context
            )
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var result = _context.Actors.Select(x => new ActorsIndexViewModel()
            {
                ActorID = x.ActorID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MoviesActedFor = x.MoviesActedFor,
                FavouriteGenre = x.FavouriteGenre,
                HasAwards = x.HasAwards
            });

            return View(result);
        }
    }
}
