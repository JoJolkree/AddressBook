using System;

namespace AddressBook.Domain
{
    public class Call
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Contact Contact { get; set; }

        public Call(Contact contact)
        {
            Contact = contact;
            Created = DateTime.Now;
        }

        protected bool Equals(Call other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Call) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}