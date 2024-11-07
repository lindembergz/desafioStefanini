using Ecommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Events;
using Ecommerce.Services;

namespace Ecommerce.Handlers
{
    public class UpdateStockHandler : IEventHandler<PurchaseCompletedEvent>
    {
        private readonly ProductService _productService;

        public UpdateStockHandler(ProductService productService)
        {
            _productService = productService;
        }

        public void Handle(PurchaseCompletedEvent @event)
        {
            foreach (var item in @event.Cart.Items)
            {
                item.Product.DecreaseStock(item.Quantity);
                Console.WriteLine($"Estoque atualizado para o produto '{item.Product.Name}': {item.Product.Stock}");
            }
        }
    }
}
