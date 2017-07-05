using System.Collections.Generic;

namespace Domain.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}