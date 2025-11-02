using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Actors;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IActorServices _actorServices;
        public ActorsController
            (
            FilminurkTARpe24Context context,
            IActorServices actorServices
            )
        {
            _context = context;
            _actorServices = actorServices;
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
                HasAwards = x.HasAwards,
                American = x.American
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
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
                
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var actor = await _actorServices.DetailsAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            var vm = new ActorsDeleteViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.FavouriteGenre = actor.FavouriteGenre;
            vm.HasAwards = actor.HasAwards;
            vm.American = actor.American;
            vm.EntryCreatedAt = actor.EntryCreatedAt;
            vm.EntryModifiedAt = actor.EntryModifiedAt;
            ViewBag.ActorID = actor.ActorID;
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var actor = await _actorServices.Delete(id);
            if (actor == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
