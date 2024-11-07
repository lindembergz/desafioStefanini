using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities
{
    public class Cart
    {
        private readonly List<CartItem> _items = new List<CartItem>();
        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

        public void AddProduct(Product product, int quantity)
        {
            var existingItem = _items.Find(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                _items.Add(new CartItem(product, quantity));
            }
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in _items)
                total += item.Product.Price * item.Quantity;
            return total;
        }
    }
}
