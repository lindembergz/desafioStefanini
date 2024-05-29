// Stefanini.Venda.Domain/Services/PedidoService.cs
using Stefanini.Venda.App.ViewModels;
using Stefanini.Venda.Domain.Entities;
using AutoMapper;
using Stefanini.Venda.App.Services.Interfaces;
using Stefanini.Venda.Domain.Repository.Interfaces;

namespace Stefanini.Venda.App.Services.Implementation
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        protected PedidoService()
        {
        }

        public PedidoService( IMapper mapper, IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;            
            _mapper = mapper;
        }

        public async Task<PedidoViewModel> GetPedidoById(int id)
        {
            return _mapper.Map<PedidoViewModel>(await _pedidoRepository.GetById(id));
        }

        public async Task<IEnumerable<PedidoViewModel>> GetPedidos()
        {
            return _mapper.Map<IEnumerable<PedidoViewModel>>(await _pedidoRepository.GetAll());
        }

        private PedidoEntity IntegrarPedidoItens(PedidoViewModel _pedido)
        {
             var pedido = _mapper.Map<PedidoEntity>(_pedido);
            _pedido.ItemsPedido.ForEach(item =>
            {               
                pedido.AddItemPedido(_mapper.Map<ItemPedidoEntity>(item), item.ValorUnitario);
            });
            return pedido;
        }

        public async Task CreatePedido(PedidoViewModel _pedido)
        {
            var pedido = IntegrarPedidoItens(_pedido);
            await _pedidoRepository.Insert(pedido);           
        }

        public async Task UpdatePedido(PedidoViewModel _pedido)
        {
           var pedido = IntegrarPedidoItens(_pedido);
            await _pedidoRepository.Update(pedido);
        }

        public async Task PayPedido(int id)
        {
            var pedido = await _pedidoRepository.GetById(id);

            pedido.UpdateToPay();

            await _pedidoRepository.Update(pedido);
        }       

        public async Task DeletePedido(int id)
        {
            var pedido = await _pedidoRepository.GetById(id);
            if (pedido != null)
            {
                await _pedidoRepository.Delete(pedido);
            }
        }
    }
}