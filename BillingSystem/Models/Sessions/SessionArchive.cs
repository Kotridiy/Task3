using ATS.Models;
using BillingSystem.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.Models.Sessions
{
    public class SessionArchive
    {
        public Dictionary<string, Contract> Contracts { get; private set; }
        public List<SessionHistory> History { get; private set; }

        public SessionArchive(Station station)
        {
            station.SessionEnd += OnSessionEnd;
            Contracts = new Dictionary<string, Contract>();
            History = new List<SessionHistory>();
        }

        void OnSessionEnd(object sender, Session session)
        {
            Contracts.TryGetValue(session.IncomingNumber, out var incomingContract);
            Contracts.TryGetValue(session.OutgoingNumber, out var outgoingContract);
            var sessionHistory = new SessionHistory(session, incomingContract, outgoingContract);
            History.Add(sessionHistory);
        }

        public IEnumerable<SessionHistory> GetHistory(Contract contract, bool lastMonth = false)
        {
            var history = History.Where(session => session.IncomingContract == contract || session.OutgoingContract == contract);
            if (lastMonth)
            {
                history = history.Where(session => session.Session.Start > DateTime.Now - TimeSpan.FromDays(30));
            }
            return history;
        }

        public IEnumerable<SessionHistory> FilterByCost(Contract contract, decimal cost, bool moreThen = false, bool lastMonth = false)
        {
            if (moreThen)
            {
                return GetHistory(contract, lastMonth).Where(
                    session => session.Session.Type == ATS.Enums.SessionType.Finished && session.Session.Cost >= cost
                );
            }
            else
            {
                return GetHistory(contract, lastMonth).Where(
                    session => session.Session.Type == ATS.Enums.SessionType.Finished && session.Session.Cost <= cost
                );
            }
        }

        public IEnumerable<SessionHistory> FilterByContract(Contract contract1, Contract contract2, bool lastMonth = false)
        {
            return GetHistory(contract1, lastMonth).Where(
                session => session.IncomingContract == contract2 || session.OutgoingContract == contract2
            );
        }

        public IEnumerable<SessionHistory> FilterByDuration(Contract contract, TimeSpan duration, bool moreThen = false, bool lastMonth = false)
        {
            if (moreThen)
            {
                return GetHistory(contract, lastMonth).Where(session => session.Session.Duration >= duration);
            }
            else
            {
                return GetHistory(contract, lastMonth).Where(session => session.Session.Duration <= duration);
            }
        }
    }
}
