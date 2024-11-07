using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities
{

    public class Product
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public Product(string name, decimal price, int stock)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity > Stock)
                throw new InvalidOperationException("Estoque insuficiente.");
            Stock -= quantity;
        }
    }
}
