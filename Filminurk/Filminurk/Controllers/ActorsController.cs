using Filminurk.Core.Dto;
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
                // MoviesActedFor = x.MoviesActedFor,
                FavouriteGenre = x.FavouriteGenre,
                HasAwards = x.HasAwards
            });

            return View(result);
        }

        [HttpGet]
        public IActionResult CreateUpdate()
        {
            ActorsCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);            
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActorsCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid == true)
            {
                var dto = new ActorsDTO()
                {
                    ActorID = vm.ActorID,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    NickName = vm.NickName,
                    MoviesActedFor = vm.MoviesActedFor,
                    FavouriteGenre = vm.FavouriteGenre,
                    HasAwards = vm.HasAwards,
                    American = vm.American,
                    EntryCreatedAt = vm.EntryCreatedAt,
                    EntryModifiedAt = vm.EntryModifiedAt
                };
                var result = await _actorServices.Create(dto);
            }
        }
    }
}
