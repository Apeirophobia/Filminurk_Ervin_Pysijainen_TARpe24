using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailsServices _emailsServices;
        public EmailsController(IEmailsServices emailsServices)
        {
            _emailsServices = emailsServices;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var dto = new EmailDTO()
            {
                SendToThisAddress = vm.SendToThisAddress,
                EmailSubject = vm.EmailSubject,
                EmailContent = vm.EmailContent
            };

            _emailsServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
