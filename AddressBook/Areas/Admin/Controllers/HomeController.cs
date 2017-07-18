using AddressBook.Controllers;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController<UsersRepository>
    {
        public HomeController(UsersRepository userRepo) : base(userRepo)
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            if (Repository.GetUserByLogin(User.Identity.Name).UserType != UserType.Admin)
                return RedirectToAction("Index", "Contacts");
            var users = Repository.GetAllUsers();
            return View(users);
        }

        [Authorize]
        public IActionResult SetRole(string login, int role)
        {
            if (Repository.GetUserByLogin(User.Identity.Name).UserType != UserType.Admin)
                return RedirectToAction("Index", "Contacts");
            var userType = (UserType) role;
            var user = Repository.GetUserByLogin(login);
            Repository.ChangeType(Repository.GetUserByLogin(User.Identity.Name), user, userType);
            return RedirectToAction("Index");
        }
    }
}