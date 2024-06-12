using Microsoft.Extensions.Options;
using Stefanini.Core.DomainObjects;

namespace Stefanini.Venda.Domain.Entities
{
    public class PedidoEntity : Entity<int> 
    {
        public string NomeCliente { get; private set; }
        public string EmailCliente { get; private set; }
        public bool Pago { get; private set; }
        public decimal ValorTotal { get; private set; }

        private readonly List<ItemPedidoEntity> ItensPedido;
        public IReadOnlyCollection<ItemPedidoEntity> PedidoItems => ItensPedido;
        protected PedidoEntity()
        {
            ItensPedido = new List<ItemPedidoEntity>();
        }
        public PedidoEntity(int id, string nomeCliente, string emailCliente, bool pago)
        {
            Id = id;
            NomeCliente = nomeCliente;
            EmailCliente = emailCliente;
            Pago = pago;
            ItensPedido = new List<ItemPedidoEntity>();

            Validar();
        }
        public void Validar()
        {
            Validation.ValidarSeVazio(NomeCliente, "O campo Nome do cliente não pode estar vazio");
        }

        private void CalcularValorTotal(decimal valor)
        {
            ValorTotal += valor;
        }

         public void AddItemPedido(ItemPedidoEntity item)
      
        {
            ItensPedido.Add(item);
            CalcularValorTotal(item.Quantidade * item.ValorUnitario);
        
        }

        public void UpdateToPay()
        {
            Pago = true;
        }

        
    }

}