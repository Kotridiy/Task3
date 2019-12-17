using System;
using ATS.Models;
using BillingSystem.Models.Contracts;

namespace BillingSystem.Models.Sessions
{
    public class SessionHistory
    {
        public Session Session { get; private set; }
        public Contract IncomingContract { get; private set; }
        public Contract OutgoingContract { get; private set; }

        internal SessionHistory(Session session, Contract incomingContract, Contract outgoingContract)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            IncomingContract = incomingContract ?? throw new ArgumentNullException(nameof(incomingContract));
            OutgoingContract = outgoingContract ?? throw new ArgumentNullException(nameof(outgoingContract));
        }
    }
}
