using ATS.Interfaces;

namespace BillingSystem.Interfaces
{
    public interface IPhoneBuilder
    {
        Phone CreatePhone(string number);
    }
}