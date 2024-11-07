using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Interfaces;

namespace Ecommerce.Events
{
    public class ProductAddedToCartEvent : IEvent
    {
        public Product Product { get; }
        public int Quantity { get; }

        public ProductAddedToCartEvent(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
