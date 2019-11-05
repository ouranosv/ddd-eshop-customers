using System;
using Domain.Models.Base;

namespace Domain.Models
{
    public class Customer : Entity<Guid>
    {
        public Name Name { get; private set; }
        public Email Email { get; private set; }

        public Customer(CustomerId id, Name name, Email email) : base(id)
        {
            if (name == null)
                throw new ArgumentNullException("name", "Name cannot be null");

            if (email == null)
                throw new ArgumentNullException("email", "Email cannot be null");

            Name = name;
            Email = email;
        }
    }
}
