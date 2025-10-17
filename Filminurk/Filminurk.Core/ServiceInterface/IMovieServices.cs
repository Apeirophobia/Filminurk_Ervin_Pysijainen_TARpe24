using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IMovieServices // See on interface, asub .core/ServiceInterface
    {
        Task<Movie> Create(MoviesDTO dto);
        Task<Movie> DetailsAsync(Guid Id);
        Task<Movie> Delete(Guid id);
        Task<Movie> Update(MoviesDTO dto);

    }
}
