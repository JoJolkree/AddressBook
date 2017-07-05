using Microsoft.EntityFrameworkCore;

namespace AddressBook.Domain
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Call> Calls { get; set; }
    }
}