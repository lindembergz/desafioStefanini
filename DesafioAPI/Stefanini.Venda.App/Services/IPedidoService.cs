using Stefanini.Venda.App.ViewModels;

namespace Stefanini.Venda.App.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoViewModel> GetPedidoById(int id);
        Task<IEnumerable<PedidoViewModel>> GetPedidos();
        Task CreatePedido(PedidoViewModel _pedido);
        Task UpdatePedido(PedidoViewModel _pedido);
        Task PayPedido(int id);
        Task DeletePedido(int id);
    }
}