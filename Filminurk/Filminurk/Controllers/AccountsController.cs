using System.ComponentModel;
using System.Diagnostics.Contracts;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly FilminurkTARpe24Context _context;
        private readonly IEmailsServices _emailsServices;
        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            FilminurkTARpe24Context context,
            IEmailsServices emailsServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailsServices = emailsServices;
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);
            if (userHasPassword)
            {
                RedirectToAction("ChangePassword");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("AddPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Accounts", new { email = model.Email, token = token }, Request.Scheme);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPassordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null || user.Email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid) // kontrollitakse kas modelstate on õige, ehk kõik andmed on olemas
            { // kui on, siis teostatakse ülejaanud meetodi sisu
                var user = await _userManager.FindByEmailAsync(model.Email); // otsitakse kasutaja emaili abil üles
                if (user != null) // ülejaanud toimub siis kui kasutaja EI ole tühi, ehk leitakse üles
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    // oodatakse kuni usermanager resetib kasutaja parooli
                    if (result.Succeeded) // kui resettimine on edukas siis:
                    {
                        if (await _userManager.IsLockedOutAsync(user)) // kontrollitakse kas seesama kasutaja on ennast katsetega lockouti läinud
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                            // ja võetakse lockout maha
                        }
                        await _signInManager.SignOutAsync(); // signinmanager logib kasutaja välja
                        await _userManager.DeleteAsync(user); // usermanageris kustutatakse kasutaja 
                        return RedirectToAction("ResetPasswordConfirmation", "Accounts"); 
                        // tagastatakse password reset kinnitusvaatesse
                    }
                    foreach (var error in result.Errors) // aga kui resettimine ei olnud edukas, logitakse kõik errorid välja
                    {
                        ModelState.AddModelError("", error.Description);
                        // ja listatakse modelstate juurde
                    }
                    return RedirectToAction("ResetPasswordConfirmation", "Accounts");
                    // ning tagastatakse password reset kinnitusvaatesse
                }
                await _userManager.DeleteAsync(user); // kui kasutaja ON tühi, ehk ei leita üles, tagastatakse kasutaja password reset kinnitussvaatesse
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            // kui modelstate ie ole õige, mingi väli on puudu, siis tagastatakse kasutaja  password reset kinnitussvaatesse
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccountCreated()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    ProfileType = model.ProfileType,
                    DisplayName = model.DisplayName,
                    AvatarImageID = Guid.NewGuid().ToString()
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var dto = new EmailDTO()
                    {
                        SendToThisAddress = user.Email,
                        EmailSubject = "Registration Confirmation",
                        EmailContent = "Please Confirm Your Email, Dumbass"
                    };
                    _emailsServices.SendEmail(dto);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);
                    // HOMEWORK TASK: koosta email kasutajalt pärineva aadressile saatmiseks, kasutaja saab oma postkastist kätte emaili
                    // kinnituslingiga, mille jaoks kasutatakse tokenit, siin tuleb välja kutsuda vastav, uus emaili saatmise meetod, mis saadab
                    // õige sisuga kirja
                }

                return RedirectToAction("Index", "Home");
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("Login");
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnURL)
        {
            LoginViewModel vm = new()
            {
                ReturnUrl = returnURL
            };
            return View(vm  );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnURL)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError("", "Sinu email ei ole kinnitatud palun vaata spämmikausta");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (result.Succeeded == false)
                {
                    ModelState.AddModelError("", "Kasutaja või parool on vale");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Sisselogimine ebaõnnestus, kasutaja keelatud");
                }
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                ModelState.AddModelError("", "Sisselogimine ebaõnnestus, kontakteeru administraatoriga");

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
