using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;
namespace VendingMachine.DataServices
{
    public static class SoftDrinksProvider
    {
        public static IList<SoftDrink> AvailableSoftDrinks { get; set; }

        static SoftDrinksProvider()
        {
            Initialise();
        }

        private static void Initialise()
        {
            AvailableSoftDrinks = new List<SoftDrink>();

            for (int i = 0; i < 25; i++)
            {
                (AvailableSoftDrinks as List<SoftDrink>).Add(new SoftDrink() { Description = "Cola", Id = i + 1, Name = "Cola", Price = 0.5f });
            };
        }
    }
}
