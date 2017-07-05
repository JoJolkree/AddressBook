using System;

namespace AddressBook.Exceptions
{
    public class UserAlreadyExists : Exception
    {
        public UserAlreadyExists()
        {
        }

        public UserAlreadyExists(string message) : base(message)
        {
        }

        public UserAlreadyExists(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}