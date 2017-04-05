using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using VendingMachine;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;

namespace VMTests
{
    [TestClass]
    public class VMTests
    {
        IPaymentSevice _mockPaymentService;
        IAccountService _mockAccountService;
        IProductService _mockProductService;

        [TestInitialize]
        public void Setup()
        {
            _mockProductService = A.Fake<IProductService>();
            _mockAccountService = A.Fake<IAccountService>();
            _mockPaymentService = A.Fake<IPaymentSevice>();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid pin, please try again.")]
        public void WhenVending_ThrowsWhenWrongPin()
        {
            var card = PaymentCardProvider.AvailableCards.First();
            var cardNumber = card.Number;

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).Returns(2);
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).Returns(0.50f);

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).Returns(card);
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).Returns(false);
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).Returns(true);

            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).Returns(true);
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).Returns(true);

            var vendor = new VendingMachine.VendingMachine(_mockPaymentService, _mockProductService);
            var product = vendor.Vend(cardNumber, 1000, ProductType.SoftDrink);

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Insufficient balance, please use another card.")]
        public void WhenVending_ThrowsWhenInsufficientBalance()
        {
            var card = PaymentCardProvider.AvailableCards.First();
            var cardNumber = card.Number;

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).Returns(2);
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).Returns(0.50f);

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).Returns(card);
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).Returns(true);
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).Returns(false);

            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).Returns(true);
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).Returns(true);

            var vendor = new VendingMachine.VendingMachine(_mockPaymentService, _mockProductService);
            var product = vendor.Vend(cardNumber, 1000, ProductType.SoftDrink);

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).MustHaveHappened();

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).MustHaveHappened();
            
            A.CallTo(() => _mockProductService.BuyProduct(A<ProductType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).MustNotHaveHappened();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "There are no more items. Apologies for inconvinience")]
        public void WhenVending_ThrowsWhenInsufficientCans()
        {
            var card = PaymentCardProvider.AvailableCards.First();
            var cardNumber = card.Number;

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).Returns(0);
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).Returns(0.50f);

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).Returns(card);
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).Returns(true);
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).Returns(false);

            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).Returns(true);
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).Returns(true);

            var vendor = new VendingMachine.VendingMachine(_mockPaymentService, _mockProductService);
            var product = vendor.Vend(cardNumber, 1000, ProductType.SoftDrink);

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustNotHaveHappened();
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).MustNotHaveHappened();

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).MustNotHaveHappened();

            A.CallTo(() => _mockProductService.BuyProduct(A<ProductType>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).MustNotHaveHappened();
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).MustNotHaveHappened();
        }

        [TestMethod]
        public void WhenVending_DeductsCardAccountBalance()
        {
            var card = PaymentCardProvider.AvailableCards.First();
            var cardNumber = card.Number;

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).Returns(10);
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).Returns(0.50f);

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).Returns(card);
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).Returns(true);
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).Returns(true);

            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).Returns(true);
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).Returns(true);

            var vendor = new VendingMachine.VendingMachine(_mockPaymentService, _mockProductService);
            var product = vendor.Vend(cardNumber, 1000, ProductType.SoftDrink);

            A.CallTo(() => _mockProductService.GetQuantity(A<ProductType>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockProductService.GetPrice(A<ProductType>.Ignored)).MustHaveHappened();

            A.CallTo(() => _mockPaymentService.GetPaymentCard(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidatePaymentCardPin(cardNumber, 1000)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.ValidateAccountBalance(card, 0.50f)).MustHaveHappened();

            A.CallTo(() => _mockProductService.BuyProduct(A<ProductType>.Ignored)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.WithdrawMoney(card, 0.50f)).MustHaveHappened();
            A.CallTo(() => _mockPaymentService.DepositMoney(CashAccountProvider.VendorAccount, 0.50f)).MustHaveHappened();
        } 
    }
}
