using Ecommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Events;

namespace Ecommerce.Handlers
{
    public class LoggingHandler :
        IEventHandler<ProductRegisteredEvent>,
        IEventHandler<ProductAddedToCartEvent>,
        IEventHandler<PurchaseCompletedEvent>
    {
        public void Handle(ProductRegisteredEvent @event)
        {
            Console.WriteLine($"Produto registrado: {@event.Product.Name}, Preço: {@event.Product.Price}, Estoque: {@event.Product.Stock}");
        }

        public void Handle(ProductAddedToCartEvent @event)
        {
            Console.WriteLine($"Produto adicionado ao carrinho: {@event.Product.Name}, Quantidade: {@event.Quantity}");
        }

        public void Handle(PurchaseCompletedEvent @event)
        {
            Console.WriteLine($"Compra finalizada. Total: {@event.Cart.GetTotal():C}");
        }
    }
}
