using System.ComponentModel.DataAnnotations;
using AddressBookDomain.Domain;

namespace AddressBook.ViewModels
{
    public class ContactEditModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is not specified")]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public ContactEditModel() { }

        public ContactEditModel(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email;
            Note = contact.Note;
        }
    }
}