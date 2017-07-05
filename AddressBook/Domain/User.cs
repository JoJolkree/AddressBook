using System;
using System.Collections.Generic;

namespace AddressBook.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Contact> Contacts { get; set; }

        public User()
        {
        }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return $"(User) {Login}";
        }

        protected bool Equals(User other)
        {
            return string.Equals(Login, other.Login, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return (Login != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Login) : 0);
        }
    }
}