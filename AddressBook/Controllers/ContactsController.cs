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
            return View(new Contact());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Contact model)
        {
            Repository.Add(model.Name, model.PhoneNumber, model.Email, model.Note);

            return RedirectToAction("Index");
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
            return View(contact);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Contact contact)
        {
            var contactFromRepo = Repository.GetContactById(contact.Id);
            Repository.Edit(contactFromRepo, contact.Name, contact.PhoneNumber, contact.Email, contact.Note);
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
                Contacts = Repository.SearchByName(model.SearchText)
            });
        }
    }
}