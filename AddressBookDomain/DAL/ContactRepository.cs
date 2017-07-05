using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;

namespace AddressBookDomain.DAL
{
    public class ContactRepository
    {
        private readonly AddressBookContext _addressBookDb;

        public ContactRepository(AddressBookContext addressBookDb)
        {
            _addressBookDb = addressBookDb;
        }

        public void Add(User user, string name ="", string phoneNumber="", string email="", string note="")
        {
            var contact = new Contact(user, name, phoneNumber, email, note);
            Add(user, contact);
        }

        public void Add(User user, Contact contact)
        {
            _addressBookDb.Contacts.Add(contact);
            _addressBookDb.Users.First(x => Equals(x, user)).Contacts.Add(contact);
            _addressBookDb.SaveChanges();
        }

        public void Remove(Contact contact)
        {
            _addressBookDb.Contacts.Remove(contact);
            _addressBookDb.SaveChanges();
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

            _addressBookDb.Contacts.Remove(contact);
            _addressBookDb.Contacts.Add(contact);
            _addressBookDb.SaveChanges();
        }

        public void Call(Contact contact)
        {
            _addressBookDb.Calls.Add(new Call(contact));
            _addressBookDb.SaveChanges();
;        }
    }
}