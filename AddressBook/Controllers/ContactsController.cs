using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private ContactsRepository _contactsRepo;
        private UsersRepository _usersRepository;

        public ContactsController(ContactsRepository contactsRepo, UsersRepository usersRepo)
        {
            _contactsRepo = contactsRepo;
            _usersRepository = usersRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            var contacts = _contactsRepo.GetAllContactsForUser(User.Identity.Name);
            return View(new UserContactsModel() {Contacts = contacts});
        }

        [Authorize]
        public IActionResult Add(UserContactsModel model)
        {
            var user = _usersRepository.GetUserByLogin(User.Identity.Name);
            _contactsRepo.Add(user, model.Name, model.PhoneNumber, model.Email, model.Note);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Remove(int id)
        {
            var user = _usersRepository.GetUserByLogin(User.Identity.Name);
            _contactsRepo.Remove(user, _contactsRepo.GetContactById(id));
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Call(int id)
        {
            var user = _usersRepository.GetUserByLogin(User.Identity.Name);
            _contactsRepo.Call(user, _contactsRepo.GetContactById(id));
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
            var user = _usersRepository.GetUserByLogin(User.Identity.Name);
            var contactFromRepo = _contactsRepo.GetContactById(contact.Id);
            _contactsRepo.Edit(user, contactFromRepo, contact.Name, contact.PhoneNumber, contact.Email, contact.Note);
            return RedirectToAction("Index");
        }
    }
}