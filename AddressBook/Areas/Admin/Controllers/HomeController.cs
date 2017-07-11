using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private UsersRepository _userRepo;
        
        public HomeController(UsersRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (_userRepo.GetUserByLogin(User.Identity.Name).UserType != UserType.Admin)
                return RedirectToAction("Index", "Contacts");
            var users = _userRepo.GetAllUsers();
            return View(users);
        }

        [Authorize]
        public IActionResult SetRole(string login, int role)
        {
            if (_userRepo.GetUserByLogin(User.Identity.Name).UserType != UserType.Admin)
                return RedirectToAction("Index", "Contacts");
            var userType = (UserType) role;
            var user = _userRepo.GetUserByLogin(login);
            _userRepo.ChangeType(_userRepo.GetUserByLogin(User.Identity.Name), user, userType);
            return RedirectToAction("Index");
        }
    }
}