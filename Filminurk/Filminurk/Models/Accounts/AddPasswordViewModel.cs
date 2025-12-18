using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma uus parool")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Kirjuta oma parool uuesti:")]
        [Compare("NewPassword", ErrorMessage = "Paroolid ei kattu, palun proovi uuesti")]
        public string ConfirmNewPassword { get; set; }
    }
}
