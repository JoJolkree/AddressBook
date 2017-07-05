﻿using System.Collections.Generic;

namespace Domain.Domain
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public List<Call> Calls { get; set; }
        public User User { get; set; }
    }
}