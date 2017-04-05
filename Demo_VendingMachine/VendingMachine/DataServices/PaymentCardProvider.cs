using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;

namespace VendingMachine.DataServices
{
    public static class PaymentCardProvider
    {
        public static IList<PaymentCard> AvailableCards { get; set; }

        static PaymentCardProvider()
        {
            Initialise();
        }

        private static void Initialise()
        {
            AvailableCards = new List<PaymentCard>();

            for (int i = 0; i < 2; i++)
            {
                AvailableCards.Add(new PaymentCard() { Account = CashAccountProvider.CustomerAccount, Number = i + 1, Pin = 1 + i });
            };

            AvailableCards.Add(new PaymentCard() { Account = CashAccountProvider.VendorAccount, Number = 3, Pin = 9999 });
        }
    }
}
