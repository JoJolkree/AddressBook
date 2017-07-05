using System.Collections.Generic;
using System.Linq;
using AddressBookDomain.DAL;
using AddressBookDomain.Domain;
using AddressBookDomain.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AddressBookDomain.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void AddAndSaveUserTest()
        {
            var data = new List<User> { new User("test", "test") }.AsQueryable();

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<AddressBookContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var userRepo = new UserRepository(mockContext.Object);
            userRepo.Add("test2", "test");

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void TestAddingUsersToDb()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase();
            var options = builder.Options;

            using (var context = new AddressBookContext(options))
                context.Database.EnsureCreated();

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                userRepo.Add("test", "test");
                userRepo.Add(new User("test2", "test2"));
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                var user1 = userRepo.GetUserByLogin("test");
                var user2 = userRepo.GetUserByLogin("test2");
                Assert.AreEqual(user1, new User("test", "test"));
                Assert.AreEqual(user2, new User("test2", ""));
            }
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            var builder = new DbContextOptionsBuilder<AddressBookContext>();
            builder.UseInMemoryDatabase();
            var options = builder.Options;
            var idToDelete = 0;
            User user3;

            using (var context = new AddressBookContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                userRepo.Add("test", "test");
                userRepo.Add("test2", "test");
                user3 = new User("test3", "Test");
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                Assert.AreEqual(userRepo.GetUserByLogin("test"), new User("test", ""));
                var user2 = userRepo.GetUserByLogin("test2");
                idToDelete = user2.Id;
                Assert.AreEqual(user2, new User("test2", ""));
                Assert.AreEqual(user3, new User("Test3", ""));
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                userRepo.Delete("test");
                userRepo.Delete(idToDelete);
                userRepo.Delete(user3);
            }

            using (var context = new AddressBookContext(options))
            {
                var userRepo = new UserRepository(context);
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test"));
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test2"));
                Assert.ThrowsException<UserNotFoundException>(() => userRepo.GetUserByLogin("test3"));
            }
        }
    }
}
