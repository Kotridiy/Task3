using ATS.Enums;
using ATS.Interfaces;
using System;

namespace MTS.Models
{
    class PortMTS : Port
    {
        const decimal START_BALANCE = 15;

        public PortMTS(Guid ID) : base(ID, START_BALANCE)
        {
        }

        protected override void ReceivingAnswer(object sender, AnswerType answer)
        {
            switch (answer)
            {
                case AnswerType.StartSession:
                    Console.WriteLine("Start talking");
                    break;
                case AnswerType.NotAvailable:
                    Console.WriteLine("Phone not avaible");
                    break;
                case AnswerType.Busy:
                    Console.WriteLine("Phone busy");
                    break;
                case AnswerType.NotExist:
                    Console.WriteLine("Phone not exist");
                    break;
                case AnswerType.NotEnoughBalance:
                    Console.WriteLine("Not enough balance");
                    break;
                default:
                    break;
            }
            base.ReceivingAnswer(sender, answer);
        }

        protected override void ReceivingEnd(object sender, string number)
        {
            base.ReceivingEnd(sender, number);
            Console.WriteLine("Call is ended");
        }

        protected override void IncomingCall(object sender, string number)
        {
            base.IncomingCall(sender, number);
            Console.WriteLine("Incoming call from " + number);
        }
    }
}
