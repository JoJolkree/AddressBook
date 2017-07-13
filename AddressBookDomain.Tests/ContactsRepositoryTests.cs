using System.Linq;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressBookDomain.Tests
{
    [TestClass]
    public class ContactsRepositoryTests
    {
        [TestMethod]
        public void AddContactTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("AddContactTest");
            var options = builder.Options;

            var user = new User("test", "test", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user, "Petya");
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual(1, context.Contacts.Count());
                Assert.AreEqual(1,
                    context.Users.Include(x => x.Contacts).FirstOrDefault(x => Equals(x, user)).Contacts.Count);
                Assert.AreEqual(user, context.Contacts.Include(x => x.User).First().User);
            }
        }

        [TestMethod]
        public void AddContactAndSearchByNameTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("AddContactAndSearchByNameTest");
            var options = builder.Options;

            var user1 = new User("test1", "", UserType.User, "");
            var user2 = new User("test2", "", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user1);
                userRepo.Add(user2);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user1, "Vasechkin", "+79123456789");
                contactRepo.Add(user2, "Petechkin", "+1233455");
            }

            using (var context = new AddressBookContext(options))
            {
                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                var vasechkin = contactRepo.SearchByName(user1, "").ToList();
                var petechkin = contactRepo.SearchByName(user2, "Petechkin").ToList();
                var empty1 = contactRepo.SearchByName(user2, "Vasechkin").ToList();
                var empty2 = contactRepo.SearchByName(user1, "Petechkin").ToList();

                Assert.AreEqual(1, vasechkin.Count);
                Assert.AreEqual("Vasechkin", vasechkin[0].Name);
                Assert.AreEqual("+79123456789", vasechkin[0].PhoneNumber);
                Assert.AreEqual(1, petechkin.Count);
                Assert.AreEqual("Petechkin", petechkin[0].Name);
                Assert.AreEqual("+1233455", petechkin[0].PhoneNumber);
                Assert.AreEqual(0, empty1.Count);
                Assert.AreEqual(0, empty2.Count);
            }
        }

        [TestMethod]
        public void RemoveContactTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("RemoveContactTest");
            var options = builder.Options;
            var user = new User("test", "test", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user);

                var contactsRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactsRepo.Add(user, "Vasechkin");
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreEqual(1, context.Contacts.Count());
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                var userFromRepo = userRepo.GetUserByLogin("test");

                var contactsRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactsRepo.Remove(userFromRepo, context.Contacts.First(x => x.Name == "Vasechkin"));
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual(0, context.Contacts.Count());
                Assert.AreEqual(0, context.Users.Include(x => x.Contacts).First().Contacts.Count());
            }
        }

        [TestMethod]
        public void DeleteContactFromUsersTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("DeleteContactFromUsersTest");
            var options = builder.Options;
            var user1 = new User("test", "test", UserType.User, "");
            var user2 = new User("test2", "", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user1);
                userRepo.Add(user2);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user1, "Petya");
                contactRepo.Add(user1, "Vasya");
                contactRepo.Add(user2, "Vanya");
                contactRepo.Add(user2, "Kolya");
            }

            using (var context = new AddressBookContext(options))
            {
                var user1FromRepo = context.Users.First(x => x.Equals(user1));
                var user2FromRepo = context.Users.First(x => x.Equals(user2));

                var petyaFromRepo = context.Contacts.First(x => x.Name == "Petya");
                var vasyaFromRepo = context.Contacts.First(x => x.Name == "Vasya");
                var vanyaFromRepo = context.Contacts.First(x => x.Name == "Vanya");
                var kolyaFromRepo = context.Contacts.First(x => x.Name == "Kolya");

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Remove(user1FromRepo, petyaFromRepo);
                contactRepo.Remove(user2FromRepo, vasyaFromRepo);
                contactRepo.Remove(user1FromRepo, vanyaFromRepo);
                contactRepo.Remove(user2FromRepo, kolyaFromRepo);
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual(2, context.Contacts.Count());
                Assert.IsTrue(context.Contacts.ToList()[0].Name == "Vasya" ||
                              context.Contacts.ToList()[1].Name == "Vasya");
                Assert.IsTrue(context.Contacts.ToList()[0].Name == "Vanya" ||
                              context.Contacts.ToList()[1].Name == "Vanya");
            }
        }

        [TestMethod]
        public void SearchByNameTests()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("SearchByNameTests");
            var options = builder.Options;
            var user = new User("test", "test", UserType.User, "");
            var emptyUser = new User("empty", "", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user);
                userRepo.Add(emptyUser);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user, "AbraCadAbra");
            }

            using (var context = new AddressBookContext(options))
            {
                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                Assert.AreEqual(1, contactRepo.SearchByName(user, "abra").Count());
                Assert.AreEqual(1, contactRepo.SearchByName(user, "abracadabra").Count());
                Assert.AreEqual(0, contactRepo.SearchByName(user, "abracadabrad").Count());
                Assert.AreEqual(0, contactRepo.SearchByName(user, "cadaa").Count());
                Assert.AreEqual(1, contactRepo.SearchByName(user, "ABRACADABRA").Count());
                Assert.AreEqual(1, contactRepo.SearchByName(user, "cAd").Count());
                Assert.AreEqual(0, contactRepo.SearchByName(emptyUser, "abracadabra").Count());
            }
        }

        [TestMethod]
        public void EditContactTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("EditContactTest");
            var options = builder.Options;
            var user = new User("test", "test", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user, "Vasya");
            }

            using (var context = new AddressBookContext(options))
            {
                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Edit(user, contactRepo.SearchByName(user, "Vasya").First(), newPhoneNumber: "+79123456789");
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual("+79123456789", context.Contacts.First().PhoneNumber);
                Assert.AreEqual("Vasya", context.Contacts.First().Name);
            }
        }

        [TestMethod]
        public void CallContactTest()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("CallContactTest");
            var options = builder.Options;
            var user = new User("test", "test", UserType.User, "");

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add(user);

                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Add(user, "Vasya");
            }

            using (var context = new AddressBookContext(options))
            {
                var contactRepo = new ContactsRepository(context, new HttpContextAccessor());
                contactRepo.Call(user, contactRepo.SearchByName(user, "Vasya").First());
            }

            using (var context = new AddressBookContext(options))
            {
                Assert.AreEqual(1, context.Calls.Count());
            }
        }
    }
}