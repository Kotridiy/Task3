using ATS.Interfaces;
using BillingSystem.Models.Contracts;

namespace BillingSystem.Interfaces
{
    public abstract class ContractBuilder
    {
        public Contract CreateContract(User user, IPhoneBuilder phoneBuilder, out Phone phone)
        {
            Contract contract = new Contract(GenerateNumber(), user);
            phone = phoneBuilder.CreatePhone(contract.Number);
            return contract;
        }

        protected abstract string GenerateNumber();
    }
}
