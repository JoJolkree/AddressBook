using Microsoft.EntityFrameworkCore;

namespace AddressBookDomain.Domain
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext()
        {
        }

        public AddressBookContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Call> Calls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
        }
    }
}