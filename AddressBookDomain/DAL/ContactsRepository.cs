using System;
using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AddressBookDomain.DAL
{
    public class ContactsRepository : BaseRepository
    {

        public ContactsRepository(AddressBookContext addressBookDb, IHttpContextAccessor accessor) : base(addressBookDb,
            accessor)
        {
        }

        public void Add(User user, string name = "", string phoneNumber = "", string email = "", string note = "")
        {
            var contact = new Contact(user, name, phoneNumber, email, note);
            Add(user, contact);
        }

        public void Add(string name = "", string phoneNumber = "", string email = "", string note = "")
        {
            Add(GetCurrentUser(), name, phoneNumber, email, note);
        }

        public void Add(User user, Contact contact)
        {
            var userFromRepo = AddressBookDb.Users.First(x => Equals(x, user));
            userFromRepo.Contacts.Add(contact);
            contact.User = userFromRepo;
            AddressBookDb.SaveChanges();
        }

        public void Remove(User user, Contact contact)
        {
            var contactFromDb = AddressBookDb.Contacts.Include(x => x.Calls).FirstOrDefault(x => Equals(contact, x));
            foreach (var call in contactFromDb.Calls)
                AddressBookDb.Calls.Remove(call);
            if (!contactFromDb.User.Equals(user))
                return;
            AddressBookDb.Contacts.Remove(contactFromDb);
            AddressBookDb.SaveChanges();
        }

        public void Remove(Contact contact)
        {
            Remove(GetCurrentUser(), contact);
        }

        public IEnumerable<Contact> GetAllContactsForUser()
        {
            return GetAllContactsForUser(GetCurrentUser());
        }

        public IEnumerable<Contact> GetAllContactsForUser(string login)
        {
            return AddressBookDb
                .Users
                .Include(x => x.Contacts)
                .First(x => string.Equals(x.Login, login, StringComparison.OrdinalIgnoreCase))
                .Contacts;
        }

        public IEnumerable<Contact> GetAllContactsForUser(User user)
        {
            return AddressBookDb
                .Users
                .Include(x => x.Contacts)
                .First(x => Equals(x, user))
                .Contacts;
        }

        public IEnumerable<Contact> SearchByName(User user, string query)
        {
            return AddressBookDb
                .Users
                .Include(x => x.Contacts)
                .First(x => Equals(x, user))
                .Contacts
                .Where(x => x.Name.ToLowerInvariant()
                    .Contains(query.ToLowerInvariant()));
        }

        public IEnumerable<Contact> SearchByName(string query)
        {
            return SearchByName(GetCurrentUser(), query);
        }

        public void Edit(User user, Contact contact, string newName = null, string newPhoneNumber = null,
            string newEmail = null, string newNote = null)
        {
            if (!user.Contacts.Contains(contact))
                return;
            contact.Name = newName ?? contact.Name;
            contact.PhoneNumber = newPhoneNumber ?? contact.PhoneNumber;
            contact.Email = newEmail ?? contact.Email;
            contact.Note = newNote ?? contact.Note;

            AddressBookDb.Contacts.Update(contact);
            AddressBookDb.SaveChanges();
        }

        public void Edit(Contact contact, string newName = null, string newPhoneNumber = null,
            string newEmail = null, string newNote = null)
        {
            Edit(GetCurrentUser(), contact, newName, newPhoneNumber, newEmail, newNote);
        }

        public void Call(User user, Contact contact)
        {
            var call = new Call(contact);
            AddressBookDb.Calls.Add(call);
            contact.Calls.Add(call);
            AddressBookDb.SaveChanges();
        }

        public void Call(Contact contact)
        {
            Call(GetCurrentUser(), contact);
        }

        public Contact GetContactById(int id)
        {
            var user = AddressBookDb.Contacts.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new UserNotFoundException();
            return user;
        }
    }
}