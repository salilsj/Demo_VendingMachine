using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;
namespace VendingMachine
{
    public interface IAccountService
    {
        bool DebitAmount(CashAccount account, float amount);
        bool CreditAmount(CashAccount account, float amount);
        float GetBalance(CashAccount account);
        CashAccount GetAccount(int accountId);
    }

    public class AccountService : IAccountService
    {           
        public CashAccount GetAccount(int accountId)
        {
            return CashAccountProvider.AvailableAccounts.Single(a => a.Id == accountId);
        }

        public float GetBalance(CashAccount account)
        {
            return account.Balance;
        }

        public bool DebitAmount(CashAccount account, float amount)
        {
            if(GetBalance(account) < amount)
                throw new ApplicationException("Insufficient balance, please deposit some money into your account.");

            account.Balance -= amount;

            return true;
        }

        public bool CreditAmount(CashAccount account, float amount)
        {
            account.Balance += amount;
            return true;
        }
    }
}
