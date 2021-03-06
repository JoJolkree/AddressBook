﻿using System;
using System.Collections.Generic;

namespace AddressBookDomain.Domain
{
    public class User
    {
        public User()
        {
            Contacts = new List<Contact>();
        }

        public User(string login, string password, UserType userType, string salt)
        {
            Login = login;
            Password = password;
            UserType = userType;
            Contacts = new List<Contact>();
            Salt = salt;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public UserType UserType { get; set; }
        public List<Contact> Contacts { get; set; }

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
            if (obj.GetType() != GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Login != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Login) : 0;
        }
    }

    public enum UserType
    {
        User,
        Admin
    }
}