

using Stefanini.Venda.Domain.Entities;

namespace Stefanini.Venda.Domain.Repository.Interfaces
{
    public interface IPedidoRepository 
    {
        Task<PedidoEntity> GetById(int id);
        Task<IEnumerable<PedidoEntity>> GetAll();
        Task Insert(PedidoEntity pedido);
        Task Update(PedidoEntity pedido);
        Task Delete(PedidoEntity pedido);

    }
}
