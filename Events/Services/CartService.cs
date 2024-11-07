using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Dispatcher;
using Ecommerce.Entities;
using Ecommerce.Events;


namespace Ecommerce.Services
{
    public class CartService
    {
        private readonly Cart _cart = new Cart();
        private readonly ProductService _productService;
        private readonly EventDispatcher _dispatcher;

        public CartService(ProductService productService, EventDispatcher dispatcher)
        {
            _productService = productService;
            _dispatcher = dispatcher;
        }

        public void AddToCart(Guid productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException("Produto não encontrado.");

            if (quantity > product.Stock)
                throw new InvalidOperationException("Quantidade excede o estoque disponível.");

            _cart.AddProduct(product, quantity);
            _dispatcher.Dispatch(new ProductAddedToCartEvent(product, quantity));
        }

        public Cart GetCart()
        {
            return _cart;
        }

        public void Checkout()
        {
            // Lógica adicional pode ser adicionada aqui, como processamento de pagamento.

            _dispatcher.Dispatch(new PurchaseCompletedEvent(_cart));
        }
    }
}
