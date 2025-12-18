using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma praegune parool: ")]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma uus parool: ")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kirjuta oma uus parool uuesti: ")]
        [Compare("NewPassword", ErrorMessage = "Paroolid ei kattu, palun proovi uuesti")]
        public string ConfirmNewPassword { get; set; }
    }
}
