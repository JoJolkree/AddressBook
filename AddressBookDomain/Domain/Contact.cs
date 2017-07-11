using System.Collections.Generic;

namespace AddressBookDomain.Domain
{
    public class Contact
    {
        public Contact()
        {
            Calls = new List<Call>();
        }

        public Contact(User user, string name = "", string phoneNumber = "", string email = "", string note = "")
        {
            User = user;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Note = note;
            Calls = new List<Call>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public List<Call> Calls { get; set; }
        public User User { get; set; }

        protected bool Equals(Contact other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Contact) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}