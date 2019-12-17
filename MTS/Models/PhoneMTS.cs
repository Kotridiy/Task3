using ATS.Interfaces;
using System;

namespace MTS.Models
{
    class PhoneMTS : Phone
    {
        public PhoneMTS(string number, Port port) : base(number, port)
        {
        }

        public override string SessionNumber
        {
            get => base.SessionNumber;
            protected set
            {
                base.SessionNumber = value;
                Console.WriteLine("Number is " + value != null ? value : "clear");
            }
        }
    }
}
