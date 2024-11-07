using Ecommerce.Dispatcher;
using Ecommerce.Services;
using Ecommerce.Handlers;
using Ecommerce.Events;

class Program
{
    static void Main(string[] args)
    {
        // Configuração do Dispatcher e Registro dos Handlers
        var dispatcher = new EventDispatcher();
        var productService = new ProductService(dispatcher);
        var cartService = new CartService(productService, dispatcher);

        var loggingHandler = new LoggingHandler();
        var updateStockHandler = new UpdateStockHandler(productService);

        dispatcher.RegisterHandler<ProductRegisteredEvent>(loggingHandler);
        dispatcher.RegisterHandler<ProductAddedToCartEvent>(loggingHandler);
        dispatcher.RegisterHandler<PurchaseCompletedEvent>(loggingHandler);
        dispatcher.RegisterHandler<PurchaseCompletedEvent>(updateStockHandler);

        // Registro de Produtos
        productService.RegisterProduct("Laptop", 2500.00m, 10);
        productService.RegisterProduct("Smartphone", 1500.00m, 20);
        productService.RegisterProduct("Headphones", 300.00m, 15);

        Console.WriteLine("\nLista de Produtos:");
        foreach (var product in productService.ListProducts())
        {
            Console.WriteLine($"ID: {product.Id} | Nome: {product.Name} | Preço: {product.Price:C} | Estoque: {product.Stock}");
        }

        // Adicionando Produtos ao Carrinho
        var laptop = productService.ListProducts()[0];
        var smartphone = productService.ListProducts()[1];

        cartService.AddToCart(laptop.Id, 2);
        cartService.AddToCart(smartphone.Id, 1);

        // Finalizando a Compra
        Console.WriteLine("\nFinalizando a compra...");
        cartService.Checkout();

        Console.WriteLine("\nLista de Produtos Após Compra:");
        foreach (var product in productService.ListProducts())
        {
            Console.WriteLine($"ID: {product.Id} | Nome: {product.Name} | Preço: {product.Price:C} | Estoque: {product.Stock}");
        }
    }
}