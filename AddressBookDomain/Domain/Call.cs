using System;

namespace AddressBookDomain.Domain
{
    public class Call
    {
        public Call()
        {
        }

        public Call(Contact contact)
        {
            Contact = contact;
            Created = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Contact Contact { get; set; }

        protected bool Equals(Call other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Call) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}