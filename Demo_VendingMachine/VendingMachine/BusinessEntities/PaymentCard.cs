using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BusinessEntities
{
    public class PaymentCard
    {
        public CashAccount Account { get; set; }
        public int Number { get; set; }
        public int Pin { get; set; }
    }
}
