using System;
using ATS.Enums;

namespace ATS.Models
{
    public class Session
    {
        public string OutgoingNumber { get; private set; }
        public string IncomingNumber { get; private set; }
        public DateTime Start { get; private set; }
        public TimeSpan Duration { get; private set; }
        public decimal Cost { get; set; }
        public SessionType Type { get; private set; }

        internal Session(string outgoingNumber, string incomingNumber, SessionType type, DateTime start, TimeSpan duration)
        {
            OutgoingNumber = outgoingNumber ?? throw new ArgumentNullException(nameof(outgoingNumber));
            IncomingNumber = incomingNumber ?? throw new ArgumentNullException(nameof(incomingNumber));
            Type = type;
            Start = start;
            Duration = duration;
        }

        internal Session(string outgoingNumber, string incomingNumber, SessionType type)
        {
            OutgoingNumber = outgoingNumber ?? throw new ArgumentNullException(nameof(outgoingNumber));
            IncomingNumber = incomingNumber ?? throw new ArgumentNullException(nameof(incomingNumber));
            Type = (type != SessionType.Finished || type != SessionType.Running) ? type 
                : throw new ArgumentException($"{nameof(type)} can't be {SessionType.Finished} or {SessionType.Running}");
            if (type != SessionType.Waiting)
            {
                Start = DateTime.Now;
            }
        }

        internal void StartSession()
        {
            if (Type == SessionType.Waiting)
            {
                Type = SessionType.Running;
                Start = DateTime.Now;
            }
            else
            {
                throw new Exception("You can't start non-waiting session.");
            }
        }

        internal void EndSession(decimal cost)
        {
            if (Type == SessionType.Running)
            {
                Type = SessionType.Finished;
                Duration = DateTime.Now - Start;
                Cost = cost;
            }
            else
            {
                throw new Exception("You can't finish non-running session.");
            }
        }

        internal void PassSession()
        {
            if (Type == SessionType.Waiting)
            {
                Type = SessionType.Passed;
                Start = DateTime.Now;
            }
            else
            {
                throw new Exception("You can't set passing non-waiting session.");
            }
        }

        internal void EmptySession()
        {
            if (Type == SessionType.Waiting)
            {
                Type = SessionType.Empty;
                Start = DateTime.Now;
            }
            else
            {
                throw new Exception("You can't set empty non-waiting session.");
            }
        }
    }
}