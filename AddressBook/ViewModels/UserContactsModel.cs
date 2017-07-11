using System.Collections.Generic;
using AddressBookDomain.Domain;

namespace AddressBook.ViewModels
{
    public class UserContactsModel
    {
        public string SearchText { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}