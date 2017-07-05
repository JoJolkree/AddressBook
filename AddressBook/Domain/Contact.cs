using System.Collections.Generic;

namespace AddressBook.Domain
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public List<Call> Calls { get; set; }
        public User User { get; set; }

        public Contact(User user, string name = "", string phoneNumber = "", string email = "", string note = "")
        {
            User = user;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Note = note;
        }

        protected bool Equals(Contact other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Contact) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}