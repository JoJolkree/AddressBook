using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;

namespace AddressBookDomain.DAL
{
    public class CallsRepository
    {
        private readonly AddressBookContext _addressBookDb;

        public CallsRepository(AddressBookContext addressBookContext)
        {
            _addressBookDb = addressBookContext;
        }

        public IEnumerable<Call> GetCallsToContact(Contact contact)
        {
            return _addressBookDb.Calls.Where(x => Equals(x.Contact, contact));
        }

        public void Remove(Call call)
        {
            _addressBookDb.Calls.Remove(call);
            _addressBookDb.SaveChanges();
        }
    }
}