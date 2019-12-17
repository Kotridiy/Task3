using System;

namespace BillingSystem.Models.Contracts
{
    public class Contract
    {
        public Guid ID { get; private set; }
        public string Number { get; private set; }
        public User User { get; private set; }

        internal protected Contract(string number, User user)
        {
            ID = Guid.NewGuid();
            Number = number ?? throw new ArgumentNullException(nameof(number));
            User = user ?? throw new ArgumentNullException(nameof(user));
        } 
    }
}
