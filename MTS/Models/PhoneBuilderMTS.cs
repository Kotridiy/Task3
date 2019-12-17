using ATS.Interfaces;
using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTS.Models
{
    class PhoneBuilderMTS : IPhoneBuilder
    {
        public Phone CreatePhone(string number)
        {
            Port port = new PortMTS(Guid.NewGuid());
            return new PhoneMTS(number, port);
        }
    }
}
