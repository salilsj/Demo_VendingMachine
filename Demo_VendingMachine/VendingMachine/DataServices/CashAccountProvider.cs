using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;

namespace VendingMachine.DataServices
{
    public static class CashAccountProvider
    {
        public static IList<CashAccount> AvailableAccounts { get; set; }

        public static CashAccount VendorAccount
        {
            get
            {
                return AvailableAccounts.Single(a => a.Id == 1);
            }
        }

        public static CashAccount CustomerAccount
        {
            get
            {
                return AvailableAccounts.Single(a => a.Id == 2);
            }
        }

        static CashAccountProvider()
        {
            Initialise();
        }

        private static void Initialise()
        {
            AvailableAccounts = new List<CashAccount>();

            for (int i = 0; i < 2; i++)
            {
                AvailableAccounts.Add(new CashAccount() { Balance = 5.00f, Id = i + 1 });
            };
        }
    }
}
