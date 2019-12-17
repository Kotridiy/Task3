using System;
using BillingSystem.Models.Sessions;
using BillingSystem.Models.Contracts;
using ATS.Models;
using BillingSystem.Interfaces;
using MTS.Models;

namespace MTS
{
    public class OperatorMTS
    {
        const decimal TARIFF = 3;
        public SessionArchive Archive { get; private set; }
        public Station Station { get; private set; }
        public ContractBuilder ContractBuilder { get; private set; }

        public OperatorMTS()
        {
            Station = new Station(TARIFF);
            Archive = new SessionArchive(Station);
            ContractBuilder = new ContractBuilderMTS();
        }
    }
}
