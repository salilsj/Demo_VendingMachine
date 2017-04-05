using System;
using System.Linq;
using System.Runtime.CompilerServices;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;

[assembly: InternalsVisibleTo("VMTests")]

namespace VendingMachine
{
    public interface IPaymentSevice
    {
        bool ValidatePaymentCardPin(int cardNumber, int pin);
        PaymentCard GetPaymentCard(int cardNumber, int pin);
        bool ValidateAccountBalance(PaymentCard card, float amount);
        bool WithdrawMoney(PaymentCard card, float amount);
        bool DepositMoney(CashAccount account, float amount);
    }
    
    public class PaymentSevice : IPaymentSevice
    {
        IAccountService _accountService;

        internal PaymentSevice() : this(new AccountService()) { }

        internal PaymentSevice(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public PaymentCard GetPaymentCard(int cardNumber, int pin)
        {
            var card = PaymentCardProvider.AvailableCards.SingleOrDefault(c => c.Number == cardNumber && c.Pin == pin);
            if(card == null)
            {
                throw new ApplicationException("Invalid card, please try again.");
            }

            return card;
        }

        public bool ValidatePaymentCardPin(int cardNumber, int pin)
        {
            try
            {
                var card = GetPaymentCard(cardNumber, pin);
                return true;
            }
            catch 
            {
                return false;
            }            
        }

        public bool ValidateAccountBalance(PaymentCard card, float amount)
        {
            return _accountService.GetBalance(card.Account) >= amount;
        }

        public bool WithdrawMoney(PaymentCard card, float amount)
        {
            if (!ValidateAccountBalance(card, amount))
            {
                throw new ApplicationException("Insufficient balance, please use another card.");
            }

            _accountService.DebitAmount(card.Account, amount);
            return true;
        }

        public bool DepositMoney(CashAccount account, float amount)
        {
            _accountService.CreditAmount(account, amount);
            return true;
        }
    }
}
