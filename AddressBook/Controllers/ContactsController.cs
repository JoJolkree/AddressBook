using AddressBook.ViewModels;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactsRepository _contactsRepo;
        private readonly UsersRepository _usersRepository;

        public ContactsController(ContactsRepository contactsRepo, UsersRepository usersRepo)
        {
            _contactsRepo = contactsRepo;
            _usersRepository = usersRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            var contacts = _contactsRepo.GetAllContactsForUser();
            return View(new UserContactsModel {Contacts = contacts});
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new Contact());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Contact model)
        {
            _contactsRepo.Add(model.Name, model.PhoneNumber, model.Email, model.Note);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Remove(int id)
        {
            _contactsRepo.Remove(_contactsRepo.GetContactById(id));
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Call(int id)
        {
            _contactsRepo.Call(_contactsRepo.GetContactById(id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Contact contact;
            try
            {
                contact = _contactsRepo.GetContactById(id);
            }
            catch (UserNotFoundException)
            {
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Contact contact)
        {
            var contactFromRepo = _contactsRepo.GetContactById(contact.Id);
            _contactsRepo.Edit(contactFromRepo, contact.Name, contact.PhoneNumber, contact.Email, contact.Note);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Search(UserContactsModel model)
        {
            if (string.IsNullOrEmpty(model.SearchText))
                return RedirectToAction("Index");
            return View("Index", new UserContactsModel
            {
                SearchText = model.SearchText,
                Contacts = _contactsRepo.SearchByName(_usersRepository.GetUserByLogin(User.Identity.Name),
                    model.SearchText)
            });
        }
    }
}