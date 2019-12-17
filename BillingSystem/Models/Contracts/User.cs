using System;

namespace BillingSystem.Models.Contracts
{
    public class User
    {
        public Guid ID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public User(string firstName, string lastName)
        {
            ID = Guid.NewGuid();
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }
    }
}
