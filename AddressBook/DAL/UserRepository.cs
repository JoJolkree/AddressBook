using System.Linq;
using AddressBook.Domain;
using AddressBook.Exceptions;

namespace AddressBook.DAL
{
    public class UserRepository
    {
        private readonly AddressBookContext _addressBookDb;

        public UserRepository(AddressBookContext addressBookDb)
        {
            _addressBookDb = addressBookDb;
        }

        public void Add(User user)
        {
            if(_addressBookDb.Users.Contains(user))
                throw new UserAlreadyExists();
            _addressBookDb.Users.Add(user);
        }

        public void Add(string login, string password)
        {
            var user = new User(login, password);
            Add(user);
        }

        public void Delete(int id)
        {
            var user = _addressBookDb.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
                Delete(user);
        }

        public void Delete(string login)
        {
            var user = _addressBookDb.Users.FirstOrDefault(x => x.Login == login);
            if(user != null)
                Delete(user);
        }

        public void Delete(User user)
        {
            _addressBookDb.Users.Remove(user);
        }

        internal void AddContact(User user, Contact contact)
        {
            user.Contacts.Add(contact);
        }
    }
}