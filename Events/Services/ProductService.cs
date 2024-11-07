using Ecommerce.Entities;
using Ecommerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Dispatcher;

namespace Ecommerce.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>();
        private readonly EventDispatcher _dispatcher;

        public ProductService(EventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void RegisterProduct(string name, decimal price, int stock)
        {
            var product = new Product(name, price, stock);
            _products.Add(product);
            _dispatcher.Dispatch(new ProductRegisteredEvent(product));
        }

        public IReadOnlyList<Product> ListProducts()
        {
            return _products.AsReadOnly();
        }

        public Product GetProductById(Guid id)
        {
            return _products.Find(p => p.Id == id);
        }
    }
}
