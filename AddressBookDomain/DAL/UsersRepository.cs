using System.Linq;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;

namespace AddressBookDomain.DAL
{
    public class UsersRepository
    {
        private readonly AddressBookContext _addressBookDb;

        public UsersRepository(AddressBookContext addressBookDb)
        {
            _addressBookDb = addressBookDb;
        }

        public void Add(User user)
        {
            if(_addressBookDb.Users.Contains(user))
                throw new UserAlreadyExistsException();
            _addressBookDb.Users.Add(user);
            _addressBookDb.SaveChangesAsync();
        }

        public void Add(string login, string password, UserType type)
        {
            var user = new User(login, password, type);
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
            _addressBookDb.SaveChangesAsync();
        }

        public User GetUserByLogin(string login)
        {
            var user = _addressBookDb.Users.FirstOrDefault(x => x.Login == login);
            if(user == null)
                throw new UserNotFoundException();
            return user;
        }
    }
}