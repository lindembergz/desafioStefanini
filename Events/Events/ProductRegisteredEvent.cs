using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Interfaces;

namespace Ecommerce.Events
{
    public class ProductRegisteredEvent : IEvent
    {
        public Product Product { get; }
        public ProductRegisteredEvent(Product product)
        {
            Product = product;
        }
    }
}
