using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
