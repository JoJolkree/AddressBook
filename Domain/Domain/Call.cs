using System;

namespace Domain.Domain
{
    public class Call
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Contact Contact { get; set; }
    }
}