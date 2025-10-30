using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;

namespace Filminurk.ApplicationServices.Services
{
    public class ActorServices : IActorServices
    {
        private readonly FilminurkTARpe24Context _context;

        public ActorServices(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public async Task<Actor> Create(ActorsDTO dto)
        {
            Actor actor = new Actor();
            actor.ActorID = Guid.NewGuid();
            actor.FirstName = dto.FirstName;
            actor.LastName = dto.LastName;
            actor.NickName = dto.NickName;
            actor.MoviesActedFor = dto.MoviesActedFor;
            actor.FavouriteGenre = dto.FavouriteGenre;
            actor.HasAwards = dto.HasAwards;
            actor.EntryCreatedAt = DateTime.Now;
            actor.EntryModifiedAt = DateTime.Now;

            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();

            return actor;
        }
    }
}
