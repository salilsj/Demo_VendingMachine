using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;

namespace VendorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine);

            foreach (var item in PaymentCardProvider.AvailableCards)
            {
                Console.WriteLine("Available Card " + item.Number + " .. Pin: " + item.Pin + " .. Balance: " + item.Account.Balance);
            }

            Console.WriteLine("Vendor Balance " + CashAccountProvider.VendorAccount.Balance);

            Console.WriteLine(Environment.NewLine + "---------------------------------------------------------------");

            Console.WriteLine(Environment.NewLine + "Enter to buy a Soft Drink");
            var totalCount = SoftDrinksProvider.AvailableSoftDrinks.Count();

            // for (int i = 0; i < totalCount + 5; i++)
            while (1 == 1)
            {
                try
                {
                    Console.WriteLine("Please enter card number to buy a soft drink");
                    int cardnumber = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Please enter card pin");
                    int cardPin = Int32.Parse(Console.ReadLine());

                    var vendor = new VendingMachine.VendingMachine();
                    var product = vendor.Vend(cardnumber, cardPin, ProductType.SoftDrink);


                    var customerCard = PaymentCardProvider.AvailableCards.Single(c => c.Number == cardnumber);

                    Console.WriteLine("Available Card  " + customerCard.Number + " .. Pin: " + customerCard.Pin + " .. Balance: " + customerCard.Account.Balance);
                    Console.WriteLine("Vendor  Balance " + CashAccountProvider.VendorAccount.Balance);
                    Console.WriteLine("Product Balance " + SoftDrinksProvider.AvailableSoftDrinks.Count());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR .. Could not complete transaction." + Environment.NewLine + "ERROR DETAILS: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine(Environment.NewLine + "---------------------------------------------------------------");
                }
            }
        }
    }
}
