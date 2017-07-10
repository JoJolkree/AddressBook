using System.ComponentModel.DataAnnotations;

namespace AddressBook.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login is not specified")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password isn't specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are not the same")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}