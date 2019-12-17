using System;

namespace ATS.Interfaces
{
    public abstract class Phone
    {
        public string Number { get; private set; }
        public bool Power { get; private set; }
        public Port Port { get; private set; }
        public virtual string SessionNumber { get; internal protected set; }

        public decimal Balance { get => Port.Balance; } 

        protected Phone(string number, Port port)
        {
            Port = port;
            port.Phone = this;
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }

        public void SwitchPower()
        {
            Power = !Power;
            if (!Power)
            {
                SessionNumber = null;
            }
            Port.ChangingPower();
        }

        public void Call(string number)
        {
            if (Power)
            {
                Port.Call(number);
                SessionNumber = number;
            }
        }

        public void TakeUpRing()
        {
            if (Power)
            {
                Port.Answer(true);
            }
        }

        public void EndCall()
        {
            if (Power)
            {
                Port.EndCall();
                SessionNumber = null;
            }
        }
    }
}