using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AddressBookDomain.DAL
{
    public class CallsRepository : BaseRepository
    {

        public CallsRepository(AddressBookContext addressBookContext, IHttpContextAccessor accessor) : base(
            addressBookContext, accessor)
        {
        }

        public IEnumerable<Call> GetCallsToContact(User user, Contact contact)
        {
            var contactFromDb = AddressBookDb.Contacts.Include(x => x.User).First(x => x.Equals(contact));

            if (contactFromDb.User.Equals(user))
                return AddressBookDb.Calls.Include(x => x.Contact).Where(x => Equals(x.Contact, contact));
            return new List<Call>();
        }

        public IEnumerable<Call> GetCallsToContact(Contact contact)
        {
            return GetCallsToContact(GetCurrentUser(), contact);
        }

        public IEnumerable<Call> GetAllUserCalls(User user)
        {
            return AddressBookDb.Users
                .Include(x => x.Contacts)
                .ThenInclude(x => x.Calls)
                .First(x => Equals(user, x))
                .Contacts
                .SelectMany(x => x.Calls);
        }

        public IEnumerable<Call> GetAllUserCalls()
        {
            return GetAllUserCalls(GetCurrentUser());
        }

        public Call GetCallByid(int id)
        {
            return AddressBookDb.Calls.First(x => x.Id == id);
        }

        public void Remove(User user, Call call)
        {
            var contactForCall = AddressBookDb.Calls.Include(x => x.Contact).First(x => x.Equals(call)).Contact;
            var isUserHasContact = AddressBookDb.Users.Include(x => x.Contacts).First(x => x.Equals(user)).Contacts
                .Contains(contactForCall);

            if (!isUserHasContact)
                return;
            AddressBookDb.Calls.Remove(call);
            AddressBookDb.SaveChanges();
        }

        public void Remove(Call call)
        {
            Remove(GetCurrentUser(), call);
        }
    }
}