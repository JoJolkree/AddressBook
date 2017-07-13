using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressBookDomain.Domain;
using Microsoft.AspNetCore.Http;

namespace AddressBookDomain.DAL
{
    public abstract class BaseRepository
    {
        private readonly AddressBookContext _addressBookDb;
        private readonly IHttpContextAccessor _accessor;

        protected BaseRepository(AddressBookContext context, IHttpContextAccessor accessor)
        {
            _addressBookDb = context;
            _accessor = accessor;
        }

        public User GetCurrentUser()
        {
            var userName = _accessor.HttpContext.User.Identity.Name;
            return _addressBookDb.Users.First(x => Equals(x.Login, userName));
        }
    }
}
