using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Dto.OMDB;

namespace Filminurk.Core.ServiceInterface
{
    public interface IOMDBServices
    {
        public Task<OMDBSearchResultDTO> OMDBResult(OMDBSearchResultDTO dto);
    }
}
