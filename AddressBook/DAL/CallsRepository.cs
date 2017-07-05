using System.Collections.Generic;
using System.Linq;
using AddressBook.Domain;

namespace AddressBook.DAL
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
            return _addressBookDb.Calls.Where(x => x.Contact == contact);
        }

        public void Remove(Call call)
        {
            _addressBookDb.Calls.Remove(call);
        }

        internal void Add(Contact contact)
        {
            _addressBookDb.Calls.Add(new Call(contact));
        }
    }
}