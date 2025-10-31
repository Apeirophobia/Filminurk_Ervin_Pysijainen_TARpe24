using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IActorServices
    {
        Task<Actor> Create(ActorsDTO dto);
        Task<Actor> DetailsAsync(Guid id);
        Task<Actor> Delete(Guid id);
    }
}
