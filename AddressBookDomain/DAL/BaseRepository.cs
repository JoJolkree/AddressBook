using System.Linq;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Http;

namespace AddressBookDomain.DAL
{
    public abstract class BaseRepository
    {
        private readonly IHttpContextAccessor _accessor;
        protected readonly AddressBookContext AddressBookDb;

        protected BaseRepository(AddressBookContext context, IHttpContextAccessor accessor)
        {
            AddressBookDb = context;
            _accessor = accessor;
        }

        public User GetCurrentUser()
        {
            var userName = _accessor.HttpContext.User.Identity.Name;
            return AddressBookDb.Users.First(x => Equals(x.Login, userName));
        }
    }
}