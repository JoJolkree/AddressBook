using System;
using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AddressBookDomain.DAL
{
    public class UsersRepository : BaseRepository
    {

        public UsersRepository(AddressBookContext addressBookDb, IHttpContextAccessor accessor) : base(addressBookDb,
            accessor)
        {
        }

        public void Add(User user)
        {
            if (AddressBookDb.Users.Contains(user))
                throw new UserAlreadyExistsException();
            AddressBookDb.Users.Add(user);
            AddressBookDb.SaveChanges();
        }

        public void Add(string login, string password, UserType type, string salt)
        {
            var user = new User(login, password, type, salt);
            Add(user);
        }

        public void Delete(int id)
        {
            var user = AddressBookDb.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
                Delete(user);
        }

        public void Delete(string login)
        {
            var user = AddressBookDb.Users.FirstOrDefault(x => x.Login == login);
            if (user != null)
                Delete(user);
        }

        public void Delete(User user)
        {
            AddressBookDb.Users.Remove(user);
            AddressBookDb.SaveChangesAsync();
        }

        public User GetUserByLogin(string login)
        {
            var user = AddressBookDb.Users.FirstOrDefault(
                x => string.Equals(x.Login, login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new UserNotFoundException();
            return user;
        }

        public User GetUserByLoginAndPassword(string login, string password)
        {
            var user = AddressBookDb.Users.FirstOrDefault(x => string.Equals(x.Login, login,
                                                                    StringComparison.OrdinalIgnoreCase) && x.Password ==
                                                                password);
            return user;
        }

        public void ChangeType(User user, User destUser, UserType type)
        {
            if (user.UserType != UserType.Admin ||
                AddressBookDb.Users.FirstOrDefault(x => Equals(x, user)) == null) return;
            AddressBookDb.Users.First(x => Equals(x, destUser)).UserType = type;
            AddressBookDb.SaveChanges();
        }

        public void ChangeType(User destUser, UserType type)
        {
            ChangeType(GetCurrentUser(), destUser, type);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return AddressBookDb.Users;
        }
    }
}