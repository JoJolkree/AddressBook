using System.ComponentModel.DataAnnotations;

namespace AddressBook.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login is not specified")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}