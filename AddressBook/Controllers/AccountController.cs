using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AddressBook.ViewModels;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsersRepository userRepo;

        public AccountController(UsersRepository repo)
        {
            userRepo = repo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepo.GetUserByLoginAndPassword(model.Login, model.Password);

                if (user != null)
                {
                    await Authenticate(model.Login);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Login or password is incorrect");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = userRepo.GetUserByLogin(model.Login);
                }
                catch (UserNotFoundException)
                {
                    userRepo.Add(model.Login, model.Password, UserType.User);
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Login or password are incorrect");
            }
            return View(model);
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}