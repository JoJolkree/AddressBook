using System;
using System.Collections.Generic;
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
            if (_addressBookDb.Users.Contains(user))
                throw new UserAlreadyExistsException();
            _addressBookDb.Users.Add(user);
            _addressBookDb.SaveChanges();
        }

        public void Add(string login, string password, UserType type, string salt)
        {
            var user = new User(login, password, type, salt);
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
            if (user != null)
                Delete(user);
        }

        public void Delete(User user)
        {
            _addressBookDb.Users.Remove(user);
            _addressBookDb.SaveChangesAsync();
        }

        public User GetUserByLogin(string login)
        {
            var user = _addressBookDb.Users.FirstOrDefault(
                x => string.Equals(x.Login, login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new UserNotFoundException();
            return user;
        }

        public User GetUserByLoginAndPassword(string login, string password)
        {
            var user = _addressBookDb.Users.FirstOrDefault(x => string.Equals(x.Login, login,
                                                                    StringComparison.OrdinalIgnoreCase) && x.Password ==
                                                                password);
            return user;
        }

        public void ChangeType(User user, User destUser, UserType type)
        {
            if (user.UserType != UserType.Admin ||
                _addressBookDb.Users.FirstOrDefault(x => Equals(x, user)) == null) return;
            _addressBookDb.Users.First(x => Equals(x, destUser)).UserType = type;
            _addressBookDb.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _addressBookDb.Users;
        }
    }
}