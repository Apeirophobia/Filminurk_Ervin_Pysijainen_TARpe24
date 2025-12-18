using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

namespace Filminurk.Models.Accounts
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Kirjuta oma uus parool uuesti")]
        [Compare("Password", ErrorMessage = "Paroolid ei kattu, palun proovi uuesti")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
