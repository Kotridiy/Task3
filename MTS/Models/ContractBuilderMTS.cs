using BillingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTS.Models
{
    class ContractBuilderMTS : ContractBuilder
    {
        protected override string GenerateNumber()
        {
            Random r = new Random();
            return $"3-{r.Next(0, 10)}{r.Next(0, 10)}-{r.Next(0, 10)}{r.Next(0, 10)}";
        }
    }
}
