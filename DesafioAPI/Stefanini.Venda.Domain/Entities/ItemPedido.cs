using Stefanini.Core.DomainObjects;
using Stefanini.Estoque.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Stefanini.Venda.Domain.Entities
{
    public class ItemPedidoEntity : Entity<int>
    {
        public int IdPedido { get; private set; }
        public int IdProduto { get; private set; }
        public decimal Quantidade { get; private set; }        
        public decimal ValorUnitario { get; private set; }
        public virtual ProdutoEntity Produto { get;  set; }
        public virtual PedidoEntity Pedido { get; private set; }

        protected ItemPedidoEntity() { }
        public ItemPedidoEntity(int idPedido, int idProduto, decimal quantidade, decimal valorUnitario) 
        {
            IdPedido = idPedido;
            IdProduto = idProduto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public void Validar()
        {
            Validation.ValidarSeMenorQue(Quantidade, 0, "A Quantidade nao pode ser menor que zero");
        }
    }
}


