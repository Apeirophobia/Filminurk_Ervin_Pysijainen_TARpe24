using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Sisesta parool uuesti")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroolid ei kattu, palun proovi uuesti")]
        public string ConfirmPassword { get; set; }
        public string? DisplayName { get; set; }
        public bool ProfileType { get; set; } = false;

    }
}
