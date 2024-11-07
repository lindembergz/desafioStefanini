using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities
{
    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
