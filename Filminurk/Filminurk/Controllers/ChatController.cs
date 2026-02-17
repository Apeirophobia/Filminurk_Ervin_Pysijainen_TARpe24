using Filminurk.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ChatController : Controller
    {
        private readonly SignInManager<Filminurk.Core.Domain.ApplicationUser> _signInManager;

        public ChatController(SignInManager<Filminurk.Core.Domain.ApplicationUser> signInManager)
        {
            _signInManager= signInManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.myUserName = await _signInManager.UserManager.GetUserAsync(User);
            return View();
        }

        
    }
}
