using System.Linq;
using AddressBook.ViewModels;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class ContactsController : BaseController<ContactsRepository>
    {
        public ContactsController(ContactsRepository contactsRepo) : base(contactsRepo)
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            var contacts = Repository.GetAllContactsForUser();
            return View(new UserContactsModel {Contacts = contacts});
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new ContactEditModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(ContactEditModel model)
        {
            if (ModelState.IsValid)
            {
                Repository.Add(model.Name, model.PhoneNumber, model.Email, model.Note);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Remove(int id)
        {
            Repository.Remove(Repository.GetContactById(id));
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Call(int id)
        {
            Repository.Call(Repository.GetContactById(id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Contact contact;
            try
            {
                contact = Repository.GetContactById(id);
            }
            catch (UserNotFoundException)
            {
                return RedirectToAction("Index");
            }
            var contactEditModel = new ContactEditModel(contact);

            return View(contactEditModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ContactEditModel contact)
        {
            if (ModelState.IsValid)
            {
                var contactFromRepo = Repository.GetContactById(contact.Id);
                Repository.Edit(contactFromRepo, contact.Name, contact.PhoneNumber, contact.Email, contact.Note);
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(UserContactsModel model)
        {
            if (string.IsNullOrEmpty(model.SearchText))
                return RedirectToAction("Index");
            return View("Index", new UserContactsModel
            {
                SearchText = model.SearchText,
                Contacts = Repository.SearchByName(model.SearchText)
            });
        }
    }
}