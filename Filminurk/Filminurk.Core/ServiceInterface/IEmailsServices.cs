using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IEmailsServices
    {
        void SendEmail(EmailDTO dto);
    }
}
