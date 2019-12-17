using System;
using ATS.Enums;

namespace ATS.Interfaces
{
    public abstract class Port
    {
        public Guid ID { get; private set; }
        public decimal Balance { get; private set; }
        public Phone Phone { get; internal set; }
        public PortState State { get; protected set; }

        public event EventHandler<string> Calling;
        public event EventHandler<bool> Answering;
        public event EventHandler<object> EndingCall;

        protected Port(Guid ID, decimal balance)
        {
            this.ID = ID;
            Balance = balance;
            State = PortState.Off;
        }

        public virtual void ChangeBalance(decimal money)
        {
            Balance += money;
        }

        protected internal virtual void ChangingPower()
        {
            if (Phone.Power)
            {
                State = PortState.Waiting;
            }
            else
            {
                if (State != PortState.Waiting)
                {
                    EndCall();
                }
                State = PortState.Off;
            }
        }

        private void OnEvent<T>(EventHandler<T> handler, object sender, T args)
        {
            EventHandler<T> newHandler;
            lock (handler)
            {
                newHandler = handler;
            }
            newHandler?.Invoke(sender, args);
        }

        protected internal virtual void Call(string number)
        {
            if (State == PortState.Waiting)
            {
                OnEvent(Calling, this, number);
            }
        }

        protected internal virtual void Answer(bool answer)
        {
            if (State != PortState.Waiting)
            {
                OnEvent(Answering, this, answer);
            }
        }

        protected internal virtual void EndCall()
        {
            if (State == PortState.Calling || State == PortState.Waiting)
            {
                OnEvent(EndingCall, this, null);
            }
        }

        protected internal virtual void IncomingCall(object sender, string number)
        {
            if (State == PortState.Waiting)
            {
                State = PortState.Calling;
                Phone.SessionNumber = number;
            }
            else
            {
                Answer(false);
            }
        }

        protected internal virtual void ReceivingEnd(object sender, string number)
        {
            if ((State == PortState.Talking || State == PortState.Calling) && number == Phone.SessionNumber)
            {
                State = PortState.Waiting;
                Phone.SessionNumber = null;
            }
        }

        protected internal virtual void ReceivingAnswer(object sender, AnswerType answer)
        {
            if (answer == AnswerType.StartSession)
            {
                State = PortState.Talking;
            }
            else
            {
                State = PortState.Waiting;
                Phone.SessionNumber = null;
            }
        }
    }
}