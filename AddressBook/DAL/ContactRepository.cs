using System.Collections.Generic;
using System.Linq;
using AddressBook.Domain;

namespace AddressBook.DAL
{
    public class ContactRepository
    {
        private readonly AddressBookContext _addressBookDb;
        private readonly UserRepository _userRepo;
        private readonly CallsRepository _callsRepo;

        public ContactRepository(AddressBookContext addressBookDb, UserRepository userRepo, CallsRepository callsRepo)
        {
            _addressBookDb = addressBookDb;
            _userRepo = userRepo;
            _callsRepo = callsRepo;
        }

        public void Add(User user, string name ="", string phoneNumber="", string email="", string note="")
        {
            var contact = new Contact(user, name, phoneNumber, email, note);
            Add(contact);
            _userRepo.AddContact(user, contact);
        }

        public void Add(Contact contact)
        {
            _addressBookDb.Contacts.Add(contact);
        }

        public void Remove(Contact contact)
        {
            _addressBookDb.Contacts.Remove(contact);
        }

        public IEnumerable<Contact> SearchByName(string query)
        {
            return _addressBookDb.Contacts.Where(x => x.Name.ToLowerInvariant().Contains(query));
        }

        public void Edit(Contact contact, string newName=null, string newPhoneNumber=null, string newEmail=null, string newNote=null)
        {
            contact.Name = newName ?? contact.Name;
            contact.PhoneNumber = newPhoneNumber ?? contact.PhoneNumber;
            contact.Email = newEmail ?? contact.Email;
            contact.Note = newNote ?? contact.Note;
        }

        public void Call(Contact contact)
        {
            _callsRepo.Add(contact);
        }
    }
}