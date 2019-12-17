using System;
using System.Collections.Generic;
using ATS.Enums;
using ATS.Interfaces;

namespace ATS.Models
{
    public class Station
    {
        Dictionary<string, Port> numbers = new Dictionary<string, Port>();
        List<Session> sessions = new List<Session>();
        public readonly decimal Tariff;

        public Station(decimal tariff)
        {
            if (tariff < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tariff));
            }
            Tariff = tariff;
        }

        public event EventHandler<Session> SessionEnd;

        void OnCalling(object sender, string number)
        {
            var outgoing = sender as Port;
            if (outgoing.Balance < Tariff)
            {
                outgoing.ReceivingAnswer(this, AnswerType.NotEnoughBalance);
            }
            else
            {
                numbers.TryGetValue(number, out Port incoming);
                if (incoming == null)
                {
                    outgoing.ReceivingAnswer(this, AnswerType.NotExist);
                }
                else
                {
                    if (incoming.State == PortState.Waiting)
                    {
                        incoming.IncomingCall(this, number);
                        sessions.Add(new Session(outgoing.Phone.Number, number, SessionType.Waiting));
                    }
                    else
                    {
                        if (incoming.State == PortState.Off)
                        {
                            outgoing.ReceivingAnswer(this, AnswerType.NotAvailable);
                        }
                        else
                        {
                            outgoing.ReceivingAnswer(this, AnswerType.Busy);
                        }
                        OnSessionEnd(this, new Session(outgoing.Phone.Number, number, SessionType.Passed));
                    }
                }
            }
        }

        void OnAnswering(object sender, bool answer)
        {
            var port = sender as Port;
            var session = sessions.Find((s) => s.IncomingNumber == port.Phone.Number);
            if (session != null && session.Type == SessionType.Running)
            {
                numbers.TryGetValue(session.OutgoingNumber, out Port caller);
                if (answer)
                {
                    caller?.ReceivingAnswer(this, AnswerType.StartSession);
                    session.StartSession();
                }
                else
                {
                    caller?.ReceivingAnswer(this, port.State == PortState.Off ? AnswerType.NotAvailable : AnswerType.Busy);
                    session.PassSession();
                    sessions.Remove(session);
                    OnSessionEnd(this, session);
                }
            }
        }

        void OnEndingCall(object sender, object args)
        {
            var port1 = sender as Port;
            var session = sessions.Find((s) => s.IncomingNumber == port1.Phone.Number || s.OutgoingNumber == port1.Phone.Number);
            numbers.TryGetValue(session.OutgoingNumber, out Port port2);
            if (port2.State == PortState.Talking)
            {
                port2.ReceivingEnd(this, port1.Phone.Number);
                decimal cost = (decimal)Math.Floor(session.Duration.TotalMinutes) * -Tariff;
                session.EndSession(cost);
                if (session.OutgoingNumber == port1.Phone.Number)
                {
                    port1.ChangeBalance(cost);
                }
            }
            else
            {
                port2.ReceivingEnd(this, port1.Phone.Number);
                session.EmptySession();
            }
            sessions.Remove(session);
            OnSessionEnd(this, session);
        }

        void OnSessionEnd(object sender, Session session)
        {
            EventHandler<Session> handler;
            lock (SessionEnd)
            {
                handler = SessionEnd;
            }
            handler?.Invoke(sender, session);
        }

        public void Connect(string number, Port port)
        {
            port.Answering += OnAnswering;
            port.Calling += OnCalling;
            port.EndingCall += OnEndingCall;
            numbers.Add(number, port);
        }

        public void Disconnect(string number)
        {
            numbers.TryGetValue(number, out var port);
            port.Answering -= OnAnswering;
            port.Calling -= OnCalling;
            port.EndingCall -= OnEndingCall;
            numbers.Remove(number);
        }
    }
}
