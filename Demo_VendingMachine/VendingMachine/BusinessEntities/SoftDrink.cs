using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.BusinessEntities
{
    public class SoftDrink : Product
    {
        public SoftDrink()
        {
            this.Type = ProductType.SoftDrink;
        }
    }
}
