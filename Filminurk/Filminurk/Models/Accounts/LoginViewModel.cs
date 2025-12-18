using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Jäta minu sisselogimine meelde")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
        public bool ProfileType { get; set; } 
    }
}
