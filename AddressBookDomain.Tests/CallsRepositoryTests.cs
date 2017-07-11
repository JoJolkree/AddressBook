using System.Linq;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressBookDomain.Tests
{
    [TestClass]
    public class CallsRepositoryTests
    {
        [TestMethod]
        public void GetCallsToContactAndRemoveTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("GetCallsToContactAndRemoveTest");
            var options = builder.Options;
            var user = new User("test", "", UserType.User);
            var user2 = new User("test2", "", UserType.User);
            var contact = new Contact(user, "Vasya");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context);
                userRepo.Add(user);
                userRepo.Add(user2);

                var contactsRepo = new ContactsRepository(context);
                contactsRepo.Add(user, contact);
                contactsRepo.Call(user, contact);
            }

            using (var context = new AddressBookContext(options))
            {
                var callsRepo = new CallsRepository(context);
                var calls = callsRepo.GetCallsToContact(user, contact);
                var calls2 = callsRepo.GetCallsToContact(user2, contact);

                Assert.AreEqual(1, calls.Count());
                Assert.AreEqual(0, calls2.Count());
            }

            using (var context = new AddressBookContext(options))
            {
                var callsRepo = new CallsRepository(context);
                var call = context.Calls.First();
                callsRepo.Remove(user2, call);

                Assert.AreEqual(1, callsRepo.GetCallsToContact(user, contact).Count());

                callsRepo.Remove(user, call);
                Assert.AreEqual(0, callsRepo.GetCallsToContact(user, contact).Count());
            }
        }
    }
}