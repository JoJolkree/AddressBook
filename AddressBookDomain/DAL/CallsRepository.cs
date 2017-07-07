using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;
using Microsoft.EntityFrameworkCore;

namespace AddressBookDomain.DAL
{
    public class CallsRepository
    {
        private readonly AddressBookContext _addressBookDb;

        public CallsRepository(AddressBookContext addressBookContext)
        {
            _addressBookDb = addressBookContext;
        }

        public IEnumerable<Call> GetCallsToContact(User user, Contact contact)
        {
            var contactFromDb = _addressBookDb.Contacts.Include(x => x.User).First(x => x.Equals(contact));

            if (contactFromDb.User.Equals(user))
                return _addressBookDb.Calls.Include(x => x.Contact).Where(x => Equals(x.Contact, contact));
            return new List<Call>();
        }

        public void Remove(User user, Call call)
        {
            var contactForCall = _addressBookDb.Calls.Include(x => x.Contact).First(x => x.Equals(call)).Contact;
            var isUserHasContact = _addressBookDb.Users.Include(x => x.Contacts).First(x => x.Equals(user)).Contacts
                .Contains(contactForCall);

            if (!isUserHasContact)
                return;
            _addressBookDb.Calls.Remove(call);
            _addressBookDb.SaveChanges();
        }
    }
}