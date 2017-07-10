using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AddressBookDomain.Domain;

namespace AddressBook.ViewModels
{
    public class UserContactsModel
    {
        [Required(ErrorMessage = "You should specify the name")]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}