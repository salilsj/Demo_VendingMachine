using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.BusinessEntities;
using VendingMachine.DataServices;

namespace VendingMachine
{
    public interface IProductService
    {
        int GetQuantity(ProductType productType);
        float GetPrice(ProductType productType);
        Product BuyProduct(ProductType productType);
        Product BuyProduct(ProductType productType, int quantity);
    }

    public class ProductService : IProductService
    {      
        public int GetQuantity(ProductType productType)
        {
            return SoftDrinksProvider.AvailableSoftDrinks == null ? 0 : SoftDrinksProvider.AvailableSoftDrinks.Count();
        }

        public float GetPrice(ProductType productType)
        {
            return productType.Price;
        }

        public Product BuyProduct(ProductType productType)
        {
            return BuyProduct(productType, 1);
        }

        public Product BuyProduct(ProductType productType, int quantity)
        {
            if (GetQuantity(productType) > 0)
            {
                var lockObj = new Object();
                lock (lockObj)
                {
                    var boughtItem = SoftDrinksProvider.AvailableSoftDrinks.First();

                    SoftDrinksProvider.AvailableSoftDrinks.Remove(boughtItem);
                    return boughtItem;
                }
            }
            else
            {
                throw new ApplicationException("There are no more items. Apologies for inconvinience");
            }
        }
    }
}
