// Stefanini.Venda.Data/Repositories/PedidoRepository.cs
using Microsoft.EntityFrameworkCore;
using Stefanini.Venda.Data.Context;
using Stefanini.Venda.Domain.Entities;
using Stefanini.Venda.Domain.Repository.Interfaces;

namespace Stefanini.Venda.Data.Repositories.Implementation
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly VendaContext _context;

        protected PedidoRepository()
        {
        }

        public PedidoRepository(VendaContext context)
        {
            _context = context;
        }

        public async Task<PedidoEntity> GetById(int id)
        {
            return await _context.Pedidos
                        .Include(p => p.PedidoItems)
                               .ThenInclude(i => i.Produto)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PedidoEntity>> GetAll()
        {
            return await _context.Pedidos
                        .Include(p => p.PedidoItems)
                            .ThenInclude(i => i.Produto)
                                .ToListAsync();
        }

        public  async  Task Insert(PedidoEntity pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PedidoEntity pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(PedidoEntity pedido)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }
}