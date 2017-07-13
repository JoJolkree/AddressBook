using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressBookDomain.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        [TestMethod]
        public void TestAddingUsersToDb()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("TestAddingUsersToDb");
            var options = builder.Options;

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add("test", "test", UserType.User, "");
                userRepo.Add(new User("test2", "test2", UserType.Admin, ""));
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                var user1 = userRepo.GetUserByLogin("test");
                var user2 = userRepo.GetUserByLogin("test2");
                Assert.AreEqual(user1, new User("test", "test", UserType.User, ""));
                Assert.AreEqual(user2, new User("test2", "", UserType.Admin, ""));
            }
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase("TestDeleteUser");
            var options = builder.Options;
            var idToDelete = 0;
            User user3;

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Add("test", "test", UserType.Admin, "");
                userRepo.Add("test2", "test", UserType.User, "");
                user3 = new User("test3", "Test", UserType.User, "");
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                Assert.AreEqual(userRepo.GetUserByLogin("test"), new User("test", "", UserType.Admin, ""));
                var user2 = userRepo.GetUserByLogin("test2");
                idToDelete = user2.Id;
                Assert.AreEqual(user2, new User("test2", "", UserType.Admin, ""));
                Assert.AreEqual(user3, new User("Test3", "", UserType.User, ""));
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                userRepo.Delete("test");
                userRepo.Delete(idToDelete);
                userRepo.Delete(user3);
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UsersRepository(context, new HttpContextAccessor());
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test"));
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test2"));
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test3"));
            }
        }
    }
}