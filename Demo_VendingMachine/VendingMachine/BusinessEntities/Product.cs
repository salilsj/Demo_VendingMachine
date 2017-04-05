using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BusinessEntities
{
    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public ProductType Type { get; set; }
    }

    public class ProductType
    {
        private ProductType() { }

        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public static ProductType SoftDrink
        {
            get
            {
                return new ProductType()
                {
                    Description = "Soft Drink",
                    Name = "Soft Drink",
                    Price = 0.5f
                };
            }
        }

    }

}
