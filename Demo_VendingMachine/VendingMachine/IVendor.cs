using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;

[assembly: InternalsVisibleTo("VMTests")]

namespace VendingMachine
{
    public interface IVendor
    {
        Product Vend(int cardNumber, int cardPin, ProductType productType);
    }

    public class VendingMachine : IVendor
    {
        readonly IPaymentSevice _paymentService;
        readonly IProductService _productService;
        private Object thisLock = new Object();

        public VendingMachine() : this(new PaymentSevice(), new ProductService())
        {
        }

        public VendingMachine(IPaymentSevice paymentService, IProductService productService)
        {            
            _paymentService = paymentService;
            _productService = productService;            
        }

        public Product Vend(int cardNumber, int cardPin, ProductType productType)
        {
            // check product exists
            var availability = _productService.GetQuantity(productType);

            if (availability < 1)
                throw new ApplicationException("There are no more items. Apologies for inconvinience");

            // Validate Pin
            if (!_paymentService.ValidatePaymentCardPin(cardNumber, cardPin))
                throw new ApplicationException("Invalid pin, please try again.");

            var productPrice = _productService.GetPrice(productType);

            var card = _paymentService.GetPaymentCard(cardNumber, cardPin);

            // Validate Balance
            if (!_paymentService.ValidateAccountBalance(card, productPrice))
                throw new ApplicationException("Insufficient balance, please use another card.");
                        
            lock (thisLock)
            {
                var product = _productService.BuyProduct(productType);
                _paymentService.WithdrawMoney(card, productType.Price);
                _paymentService.DepositMoney(CashAccountProvider.VendorAccount, productType.Price);
                return product;
            }
        }
    }
}
