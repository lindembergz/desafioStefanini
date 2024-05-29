
using Stefanini.Venda.App.Services.Interfaces;
using Stefanini.Venda.App.ViewModels;


namespace DesafioAPI.Api.Pedido.Controllers
{
    public static class PedidoController
    {
        public static void Configure(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pedidos", async (IPedidoService pedidoService) =>
            {
                return await pedidoService.GetPedidos();
            });

            app.MapGet("/api/pedidos/{id}", async (int id, IPedidoService pedidoService) =>
            {
                return await pedidoService.GetPedidoById(id);
            });

            app.MapPost("/api/pedidos", async (PedidoViewModel pedido, IPedidoService pedidoService) =>
            {
                if (pedido == null || pedido?.Id > 0)
                {
                    return Results.BadRequest("Pedido inválido.");
                }

                if (pedido.ItemsPedido.Count == 0)
                {
                    return Results.BadRequest("Nenhum Item adicionado ao Pedido.");
                }

                await pedidoService.CreatePedido(pedido);
                return Results.Created($"/api/pedidos/{pedido.Id}", pedido);
            });

            app.MapPut("/api/pedidos/{id}", async (int id, PedidoViewModel pedido, IPedidoService pedidoService) =>
            {
                if (id != pedido.Id)
                {
                    return Results.BadRequest("ID do pedido não corresponde.");
                }

                await pedidoService.UpdatePedido(pedido);
                return Results.Ok();
            });

            //Não existe app.MapPatch.
            app.MapPut("/api/pedidos/pagar/{id}", async (int id, IPedidoService pedidoService) =>
            {
                var pedido = await pedidoService.GetPedidoById(id);
                if (pedido == null)
                {
                    return Results.NotFound("Pedido não encontrado.");
                }

                await pedidoService.PayPedido(id);
                return Results.Ok();
            });

            app.MapDelete("/api/pedidos/{id}", async (int id, IPedidoService pedidoService) =>
            {
                var pedido = await pedidoService.GetPedidoById(id);
                if (pedido == null)
                {
                    return Results.NotFound("Pedido não encontrado.");
                }

                await pedidoService.DeletePedido(id);
                return Results.NoContent();
            });
        }
    }

}
